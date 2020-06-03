using PanGu;
using System;
using System.Collections.Generic;
using System.Text;

namespace SingleDigitRule
{
    /// <summary>
    /// 单个数字的POS转换成数字
    /// 合并数字和号楼，单元等信息
    /// </summary>
    public class SingleDigit : ICustomRule
    {
        public string Text { get; set; }

        private string[] Qualifiers = new string[] { "号楼", "单元", "号内", "号外", "区", "室", "层", "号", "排", "栋", "楼", "巷", "队", "组", "宿舍" ,"弄", "片","座"};

        public void AfterSegment(SuperLinkedList<WordInfo> result)
        {
            SuperLinkedListNode<WordInfo> node = result.First;
            SuperLinkedListNode<WordInfo> previous = null;

            while (node != null)
            {
                ///寻找结尾
                int index = -1;
                for (int i = 0; i < Qualifiers.Length; i++)
                    if (Qualifiers[i] == node.Value.Word)
                    {
                        index = i;
                        break;
                    }

                if (index != -1 && node.Previous != null &&
                        (node.Previous.Value.Pos == POS.POS_D || ///数量词
                            (node.Previous.Value.Pos == POS.POS_UNK &&
                             node.Previous.Value.Word.Length == 1 &&
                             node.Previous.Value.Word[0] >= '0' &&
                             node.Previous.Value.Word[0] <= '9')))///一位数量词
                {
                    #region 合并数词和量词(使用替换字符，避免不同类型的数字匹配上)
                    //WordInfo verWord = new WordInfo(node.Previous.Value.Word + node.Value.Word, POS.POS_D, 0);
                    WordInfo verWord = new WordInfo(node.Previous.Value.Word + Convert.ToChar(index + Convert.ToByte('a')), POS.POS_D, 0);
                    verWord.Rank = 5;
                    verWord.Position = node.Previous.Value.Position;
                    verWord.WordType = WordType.Numeric;

                    previous = node.Previous.Previous;
                    node = node.Next;

                    if (previous == null)
                    {
                        result.AddFirst(verWord);
                        previous = result.First;
                    }
                    else
                    {
                        result.AddAfter(previous, verWord);
                        previous = previous.Next;
                    }

                    result.Remove(previous.Next);
                    result.Remove(previous.Next);
                    #endregion
                    #region 删除量词
                    //previous = node.Previous;///previous必不为null
                    //node = node.Next;
                    //result.Remove(previous.Next);
                    #endregion
                }
                else
                    node = node.Next;
            }
        }
    }
}
