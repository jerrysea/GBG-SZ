/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

//公司：省、市、区、公司字号、公司行业、公司类型、分支机构、其他
//地址：省、市、区（县）、街道、地址详情、门牌号

using System;
using System.Collections.Generic;
using System.Text;

namespace PanGu
{
    [Flags]
    public enum POS
    {
        /// <summary>
        /// 公司核心名称
        /// </summary>
        POS_C_N = 0x00020000,  
        /// <summary>
        /// 无意义
        /// </summary>
        POS_N_M = 0x00010000,  
        /// <summary>
        /// 地标
        /// </summary>
        POS_FLG = 0x00008000,  
        /// <summary>
        /// 公司
        ///     分支机构 - Branch
        /// </summary>
        POS_C_B = 0x00004000,

        /// <summary>
        /// 公司
        ///     公司类型 - Type
        /// </summary>
        POS_C_T = 0x00002000,

        /// <summary>
        /// 公司
        ///     公司行业 - Industry
        /// </summary>
        POS_C_I = 0x00001000,

        /// <summary>
        /// 行政区域地名
        ///     省 - Province
        /// </summary>
        POS_A_P = 0x00000800,

        /// <summary>
        /// 行政区域地名
        ///     市 - Municipality
        /// </summary>
        POS_A_MU = 0x00000400,

        /// <summary>
        /// 行政区域地名
        ///     直辖市市 - Municipality directly under the center government 
        /// </summary>
        POS_A_MD = 0x00000200,

        /// <summary>
        /// 行政区域地名
        ///     县 - County
        /// </summary>
        POS_A_C = 0x00000100,

        /// <summary>
        /// 行政区域地名
        ///     乡镇 - Town
        /// </summary>
        POS_A_T = 0x00000080,

        /// <summary>
        /// 行政区域地名
        ///     村 - Village
        /// </summary>
        POS_A_V = 0x00000040,

        /// <summary>
        /// 街巷名
        ///     街道 - Street
        /// </summary>
        POS_S_S = 0x00000020,

        /// <summary>
        /// 街巷名
        ///     社区名 - Community
        /// </summary>
        POS_S_C = 0x00000010,

        /// <summary>
        /// 量词  -   Quantifier
        /// </summary>
        POS_Q = 0x00000008,
        
        /// <summary>
        /// 数词 数语素
        /// </summary>
        POS_D = 0x00000004,	//	数词 数语素

        /// <summary>
        /// 外文字符
        /// </summary>
        POS_A_NX = 0x00000002,	//	外文字符

        /// <summary>
        /// 未知词性
        /// </summary>
        POS_UNK = 0x00000000,   //  未知词性
    }
}
