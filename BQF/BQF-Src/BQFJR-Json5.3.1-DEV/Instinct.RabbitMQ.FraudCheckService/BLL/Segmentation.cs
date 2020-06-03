using PanGu;
using PanGu.Match;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace Instinct.RabbitMQ.FraudCheckService.BLL
{
    /// <summary>
    /// 分词（公司，地址）
    /// </summary>
    public class Segmentation
    {
        static Segmentation()
        {
            Segment.Init();
        }
        /// <summary>
        /// 分词 公司
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        public static FM_Company FetchCompanyEntity(string company)
        {
            FM_Company fm = new FM_Company();
           
            Segment segment = new Segment();
            MatchOptions opts = new MatchOptions();
            opts.ChineseNameIdentify = true;
            opts.UnknownWordIdentify = false;

            bool flag1 = false;//上次是Core Name
            bool flag2 = false;//不是是第一次Core Name
            bool flag3 = false;//分支机构？
            bool flag4 = false;//公司行业
            ICollection<WordInfo> words = segment.DoSegment(company, opts);
            foreach (WordInfo word in words)
            {

                switch (word.Pos)
                {
                    case POS.POS_A_P: //省
                        if (flag1 && fm.CoreName.Length > 1) flag2 = true;
                        if (flag3 || flag4) fm.Branch += word.Word;
                        fm.Province = word.Word;
                        break;
                    case POS.POS_A_MU:
                        if (flag1 && fm.CoreName.Length > 1) flag2 = true;
                        if (flag3 || flag4) fm.Branch += word.Word;
                        fm.City = word.Word;
                        break;
                    case POS.POS_A_C://区县
                        if (flag1 && fm.CoreName.Length > 1) flag2 = true;
                        if (flag3 || flag4) fm.Branch += word.Word;
                        fm.District = word.Word;
                        break;
                    case POS.POS_C_I:
                        if (flag1 && fm.CoreName.Length > 1) flag2 = true;
                        if (flag3) fm.Branch += word.Word;
                        flag4 = true;
                        fm.Industry += word.Word + " ";
                        break;
                    case POS.POS_C_T:
                        flag2 = true;
                        flag3 = true;
                        fm.CompanyType += word.Word + " ";
                        break;
                    case POS.POS_C_B:
                        if (flag1 && fm.CoreName.Length > 1) flag2 = true;
                        fm.Branch += word.Word;
                        break;

                    case POS.POS_C_N:
                        flag3 = true;
                        flag2 = true;
                        fm.CoreName += word.Word;
                        break;
                    case POS.POS_UNK:
                    case POS.POS_FLG:
                    case POS.POS_S_S:
                        if ((flag3 || flag4) && flag2) fm.Branch += word.Word;
                        if (flag2)
                        {
                            fm.Other += word.Word;
                        }
                        else if (!flag4 || !flag2)
                        {
                            fm.CoreName += word.Word;
                            flag1 = true;
                        }
                        break;
                    default:
                        if (word.WordType == WordType.Numeric && word.GetEndPositon() > 1) fm.CoreName += word.Word;

                        break;
                }
            }
            fm.Industry = RemoveEnd(fm.Industry);
            fm.CompanyType = RemoveEnd(fm.CompanyType);
            if (fm.CoreName != null && fm.CoreName.Length <= 1) fm.CoreName = "";
            if (fm.Branch != null && fm.Branch.Length <= 2) fm.Branch = "";

            return fm;
        }

        /// <summary>
        /// 分词 地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static FM_Address FetchAddressEntity(string address)
        {
            FM_Address fm = new FM_Address();
            
            Segment segment = new Segment();
            MatchOptions opts = new MatchOptions();
            opts.ChineseNameIdentify = true;
            opts.UnknownWordIdentify = false;
            opts.FrequencyFirst = true;
            opts.MultiDimensionality = true;
            opts.UnknownWordIdentify = true;

            bool provinceFlag = false;
            bool cityFlag = false;
            bool districtFlag = false;
            ICollection<WordInfo> words = segment.DoSegment(address, opts);
            WordInfo preWord = null;//....新加
            foreach (WordInfo word in words)
            {
                switch (word.Pos)
                {
                    case POS.POS_A_P: //省d
                        if (!provinceFlag)
                            fm.Province = word.Word;
                        provinceFlag = true;
                        break;
                    case POS.POS_A_MU://市
                        if (!cityFlag)
                            fm.City = word.Word;
                        cityFlag = true;
                        break;
                    case POS.POS_A_C://区县
                        if (!districtFlag)
                            fm.District = word.Word;
                        else
                            fm.Detail += word.Word;
                        districtFlag = true;
                        break;

                    case POS.POS_S_S://街道
                        if (string.IsNullOrEmpty(fm.Street))
                        {
                            fm.Street = word.Word;
                            //fm.Detail += word.Word;
                        }
                        else
                        {
                            fm.Detail += word.Word;
                        }
                        break;
                    case POS.POS_FLG://地标
                        fm.Landmark = word.Word;
                        fm.Detail += word.Word;
                        break;
                    case POS.POS_UNK:
                    case POS.POS_D:
                        if (!word.Word.StartsWith("中国"))
                        {
                            if (word.WordType == WordType.Numeric)
                            {
                                if (preWord != null && preWord.WordType == WordType.Numeric)
                                {
                                    fm.AddressNumber = fm.AddressNumber.Substring(0, fm.AddressNumber.Length - 1);
                                }
                                fm.AddressNumber += word + "-";
                            }
                            //.................................
                            else if (word.WordType == WordType.English)
                            {
                                var wordStr = word.ToString();
                                if ((wordStr.StartsWith("O") || wordStr.StartsWith("o")) &&
                                    preWord != null && preWord.WordType == WordType.Numeric)
                                {
                                    fm.AddressNumber = fm.AddressNumber.Substring(0, fm.AddressNumber.Length - 1) +
                                            wordStr.Replace("O", "0").Replace("o", "0").ToString().ToUpper() + "-";
                                }
                                else if (Regex.Match(wordStr, "[0-9]").Length > 0)
                                {
                                    fm.AddressNumber += wordStr.Replace("O", "0").Replace("o", "0").ToString().ToUpper() + "-";
                                }
                                else
                                {
                                    fm.AddressNumber += word.ToString().ToUpper() + "-";
                                }
                            }
                            else
                            {
                                bool firstNotNumber;
                                bool notSetAsNumber;
                                var cnToIntStr = ParseCnToIntStr(word.ToString(), out firstNotNumber, out notSetAsNumber);
                                if (!string.IsNullOrEmpty(cnToIntStr))
                                {
                                    if (!notSetAsNumber)
                                    {
                                        word.WordType = WordType.Numeric;
                                    }
                                    if (preWord != null && preWord.WordType == WordType.Numeric && !firstNotNumber)
                                    {
                                        fm.AddressNumber = fm.AddressNumber.Substring(0, fm.AddressNumber.Length - 1);
                                    }
                                }
                                fm.AddressNumber += cnToIntStr;
                            }
                            //...................................
                            if (word.WordType != WordType.Symbol)
                                fm.Detail += word.Word;
                            break;
                        }
                        break;
                    default:
                        if (word.WordType == WordType.SimplifiedChinese || word.WordType == WordType.Numeric)
                        {
                            fm.Detail += word;
                        }
                        break;

                }
                preWord = word;//preWord用于存储上一个分词
            }
            fm.AddressNumber = RemoveEnd(fm.AddressNumber);

            return fm;
        }       

        private static string RemoveEnd(string words)
        {
            if (string.IsNullOrEmpty(words)) return words;
            string result = words;
            result = words.Remove(words.Length - 1);
            return result;
        }

        #region helper chinese to number
        /// <summary>  
        /// 转换数字  
        /// </summary>  
        protected static long CharToNumber(char c)
        {
            switch (c)
            {
                case '一': return 1;
                case '二': return 2;
                case '三': return 3;
                case '四': return 4;
                case '五': return 5;
                case '六': return 6;
                case '七': return 7;
                case '八': return 8;
                case '九': return 9;
                case '零': return 0;
                default: return -1;
            }
        }

        /// <summary>  
        /// 转换单位  
        /// </summary>  
        protected static long CharToUnit(char c)
        {
            switch (c)
            {
                case '十': return 10;
                case '百': return 100;
                case '千': return 1000;
                case '万': return 10000;
                case '亿': return 100000000;
                default: return 1;
            }
        }
        /// <summary>  
        /// 将中文数字转换阿拉伯数字  
        /// </summary>  
        public static string ParseCnToIntStr(string cnum, out bool firstNotNumber, out bool notSetAsNumber)
        {
            cnum = Regex.Replace(cnum, "\\s+", "");
            cnum = Regex.Replace(cnum, "[^零|一|二|三|四|五|六|七|八|九|十|百|千|万|亿]", "#");
            firstNotNumber = cnum.IndexOf("#") == 0;
            notSetAsNumber = (cnum.LastIndexOf("#") == cnum.Length - 1);
            var cnums = cnum.Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
            long firstUnit = 1;//一级单位                  
            long secondUnit = 1;//二级单位   
            long tmpUnit = 1;//临时单位变量  
            long result = 0;//结果  
            var resultStr = "";
            if (cnums.Length > 0)
            {
                notSetAsNumber = notSetAsNumber || Regex.Match(cnums[cnums.Length - 1], "[十|百|千|万|亿]").Length > 0;
                for (int j = 0; j < cnums.Length; j++)
                {
                    cnum = cnums[j];
                    firstUnit = 1;//一级单位                  
                    secondUnit = 1;//二级单位   
                    tmpUnit = 1;//临时单位变量  
                    result = 0;//结果  
                    if (Regex.Match(cnum, "[十|百|千|万|亿]").Length > 0)
                    {
                        if (Regex.Match(cnum, "[十|百|千|万|亿]").Length > 0 &&
                        Regex.Match(cnum, "[零|一|二|三|四|五|六|七|八|九]").Length > 0)
                        {
                            for (int i = cnum.Length - 1; i > -1; --i)//从低到高位依次处理  
                            {
                                tmpUnit = CharToUnit(cnum[i]);//取出此位对应的单位  
                                if (tmpUnit > firstUnit)//判断此位是数字还是单位  
                                {
                                    firstUnit = tmpUnit;//是的话就赋值,以备下次循环使用  
                                    secondUnit = 1;
                                    if (i == 0)//处理如果是"十","十一"这样的开头的  
                                    {
                                        result += firstUnit * secondUnit;
                                    }
                                    continue;//结束本次循环  
                                }
                                else if (tmpUnit > secondUnit)
                                {
                                    secondUnit = tmpUnit;
                                    continue;
                                }
                                result += firstUnit * secondUnit * CharToNumber(cnum[i]);//如果是数字,则和单位想乘然后存到结果里  
                            }
                            resultStr += result.ToString() + "-";
                        }
                        else if (Regex.Match(cnum, "[十]").Length > 0)
                        {
                            var tempResult = "";
                            for (int i = 0; i < cnum.Length; i++)
                            {
                                tempResult += CharToUnit(cnum[i]);
                            }
                            resultStr += tempResult.ToString() + "-";
                        }

                    }
                    else
                    {
                        var tempResult = "";
                        for (int i = 0; i < cnum.Length; i++)
                        {
                            tempResult += CharToNumber(cnum[i]);
                        }
                        resultStr += tempResult.ToString() + "-";
                    }
                }
            }
            return resultStr;
        }
        #endregion
    }
}
