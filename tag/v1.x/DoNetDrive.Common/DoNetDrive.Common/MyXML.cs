using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DoNetDrive.Common
{
    /// <summary>
    /// Xml 工具类
    /// </summary>
    public class MyXML
    {

        /// <summary>
        /// XML文档
        /// </summary>
        private System.Xml.XmlDocument mXMLDoc;
        /// <summary>
        /// XML根节点
        /// </summary>
        private System.Xml.XmlElement mElement;
        /// <summary>
        /// XML主节点
        /// </summary>
        private System.Xml.XmlNode mMajorNode;
        private bool mOpen;
        private const int TabSpace = 4;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MyXML()
        {
            mXMLDoc = new System.Xml.XmlDocument();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sFileName"></param>
        public MyXML(string sFileName)
        {
            mXMLDoc = new System.Xml.XmlDocument();
            LoadXMLOnFile(sFileName);
        }

        /// <summary>
        /// 返回xml 是否已打开
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return mOpen;
        }


        /// <summary>
        /// 从文件中中提取XML对象
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public bool LoadXMLOnFile(string sFileName)
        {
            try
            {
                mXMLDoc.Load(sFileName);     // 从字符串
                mElement = mXMLDoc.DocumentElement;
                mOpen = true;
                return true;
            }
            catch (Exception ex)
            {
                mOpen = false;
                return false;
            }
        }

        /// <summary>
        /// 从字符串中提取XML对象
        /// </summary>
        /// <param name="strXML"></param>
        /// <returns></returns>
        public bool LoadXMLOnString(string strXML)
        {
            try
            {
                mXMLDoc.LoadXml(strXML);    // 从字符串
                mElement = mXMLDoc.DocumentElement;
                mOpen = true;
                return true;
            }
            catch (Exception ex)
            {
                mOpen = false;
                return false;
            }
        }


        /// <summary>
        /// 选择根节点
        /// </summary>
        /// <param name="sNodes"></param>
        /// <returns></returns>
        public bool SelectDocumentElement(string sNodes)
        {
            // 选择根节点
            mElement = (XmlElement)mXMLDoc.SelectSingleNode(sNodes);

            if (mElement == null)
            {
                // 没有根节点就创建一个
                mElement = mXMLDoc.CreateElement(sNodes);
                // 添加节点到文档
                mXMLDoc.AppendChild(mElement);
                // 换行
                mElement.AppendChild(mXMLDoc.CreateTextNode(StringUtil.StringCrLf));
            }
            // mXMLDoc.documentElement = mElement

            return true;
        }

        /// <summary>
        /// 设置根节点属性值
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sValue"></param>
        public void setDocumentElementAttribute(string sName, string sValue)
        {
            if (mXMLDoc.DocumentElement == null)
                return;

            mXMLDoc.DocumentElement.SetAttribute(sName, sValue);
        }


        /// <summary>
        /// 读取根节点属性值
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sDefault_value"></param>
        /// <returns></returns>
        public string GetDocumentElementAttribute(string sName, string sDefault_value = "")
        {
            if (mXMLDoc.DocumentElement == null)
            {
                return sDefault_value;
            }

            return mXMLDoc.DocumentElement.GetAttribute(sName);
        }

        /// <summary>
        /// 删除根节点属性
        /// </summary>
        /// <param name="sName"></param>
        public void DeleteDocumentElementAttribute(string sName)
        {
            if (mXMLDoc.DocumentElement == null)
                return;

            mXMLDoc.DocumentElement.RemoveAttribute(sName);
        }



        /// <summary>
        /// 选择主节点
        /// </summary>
        /// <param name="sNodes"></param>
        /// <returns></returns>
        public bool SelectElement(string sNodes)
        {
            if (mElement == null)
                return false;

            // 选择主节点
            mMajorNode = mElement.SelectSingleNode(sNodes);

            if (mMajorNode == null)
            {
                // 没有主节点就创建一个
                mMajorNode = mXMLDoc.CreateNode(System.Xml.XmlNodeType.Element, sNodes, string.Empty);
                // 换行
                mElement.AppendChild(mXMLDoc.CreateTextNode(StringUtil.StringCrLf));
                // 添加空格
                mElement.AppendChild(mXMLDoc.CreateTextNode(Space(TabSpace)));
                // 添加到根节点的末尾
                mElement.AppendChild(mMajorNode);
                // 换行
                mElement.AppendChild(mXMLDoc.CreateTextNode(StringUtil.StringCrLf));


                // 换行
                mMajorNode.AppendChild(mXMLDoc.CreateTextNode(StringUtil.StringCrLf));
                // 添加空格
                mMajorNode.AppendChild(mXMLDoc.CreateTextNode(Space(TabSpace)));
            }

            return true;
        }

        /// <summary>
        /// 设置主节点属性值
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sValue"></param>
        public void setElementAttribute(string sName, string sValue)
        {
            if (mMajorNode == null)
                return;
            System.Xml.XmlElement oElement;
            oElement = (XmlElement)mMajorNode;
            oElement.SetAttribute(sName, sValue);
        }


        /// <summary>
        /// 读取主节点属性值
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sDefault_value"></param>
        /// <returns></returns>
        public string GetElementAttribute(string sName, string sDefault_value = "")
        {
            if (mMajorNode == null)
            {
                return sDefault_value;
            }
            System.Xml.XmlElement oElement;
            oElement = (XmlElement)mMajorNode;
            return string.Empty + oElement.GetAttribute(sName);
        }

        /// <summary>
        /// 删除主节点属性
        /// </summary>
        /// <param name="sName"></param>
        public void DeleteElementAttribute(string sName)
        {
            if (mMajorNode == null)
                return;
            System.Xml.XmlElement oElement;
            oElement = (XmlElement)mMajorNode;
            oElement.RemoveAttribute(sName);
        }

        /// <summary>
        /// 主节点赋值
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sVaues"></param>
        /// <returns></returns>
        public bool SetElementValue(string sKey, string sVaues)
        {
            System.Xml.XmlNode oNode;
            bool bAddNew = false;

            if (mElement == null)
                return false;

            // 选择子节点
            oNode = mElement.SelectSingleNode(sKey);

            if (oNode == null)
            {
                // 没有子节点就创建一个
                // 换行
                mElement.AppendChild(mXMLDoc.CreateTextNode(StringUtil.StringCrLf));
                // 添加空格
                mElement.AppendChild(mXMLDoc.CreateTextNode(Space(TabSpace)));
                // 添加节点到文档
                oNode = mXMLDoc.CreateNode(System.Xml.XmlNodeType.Element, sKey, null);

                bAddNew = true;
            }

            oNode.InnerText = sVaues;

            if (bAddNew)
            {
                // 将此节点添加到主节点的节点列表末尾
                mElement.AppendChild(oNode);
                // 换行
                mElement.AppendChild(mXMLDoc.CreateTextNode(StringUtil.StringCrLf));
                // 添加空格
                mElement.AppendChild(mXMLDoc.CreateTextNode(Space(TabSpace)));
            }

            oNode = null;

            return true;
        }

        /// <summary>
        /// 主节点取值
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sDefault_value"></param>
        /// <returns></returns>
        public string GetElementValue(string sKey, string sDefault_value = "")
        {
            System.Xml.XmlNode oNode;

            if (mElement == null)
                return sDefault_value;

            oNode = mElement.SelectSingleNode(sKey);
            string sRet;
            if (oNode == null)
                sRet = sDefault_value;
            else
                sRet = oNode.InnerText;

            oNode = null;
            return sRet;
        }


        /// <summary>
        /// 子节点赋值
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sVaues"></param>
        /// <returns></returns>
        public bool SetValue(string sKey, string sVaues)
        {
            System.Xml.XmlNode oNode;
            bool bAddNew = false;

            if (mMajorNode == null)
                return false;

            // 选择子节点
            oNode = mMajorNode.SelectSingleNode(sKey);

            if (oNode == null)
            {
                // 没有子节点就创建一个
                // 添加空格
                mMajorNode.AppendChild(mXMLDoc.CreateTextNode(Space(TabSpace * 2)));
                // 添加节点到文档
                oNode = mXMLDoc.CreateNode(System.Xml.XmlNodeType.Element, sKey, null);

                bAddNew = true;
            }

            oNode.InnerText = sVaues;

            if (bAddNew)
            {
                // 将此节点添加到主节点的节点列表末尾
                mMajorNode.AppendChild(oNode);
                // 换行
                mMajorNode.AppendChild(mXMLDoc.CreateTextNode(StringUtil.StringCrLf));
                // 添加空格
                mMajorNode.AppendChild(mXMLDoc.CreateTextNode(Space(TabSpace)));
            }

            oNode = null;

            return true;
        }

        /// <summary>
        /// 返回指定空格字符串
        /// </summary>
        /// <returns></returns>
        private static string Space(int iCount)
        {
            return new string(' ', iCount);
        }


        /// <summary>
        /// 子节点取值
        /// </summary>
        /// <param name="sKey"></param>
        /// <param name="sDefault_value"></param>
        /// <returns></returns>
        public string GetValue(string sKey, string sDefault_value = "")
        {
            System.Xml.XmlNode oNode;

            if (mMajorNode == null)
            {
                return sDefault_value;
            }

            oNode = mMajorNode.SelectSingleNode(sKey);
            string ret;
            if (oNode == null)
                ret = sDefault_value;
            else
                ret = oNode.InnerText;

            oNode = null;

            return ret;
        }


        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="sName"></param>
        public void DeleteValue(string sName)
        {
            if (mMajorNode == null)
                return;

            System.Xml.XmlNode oNode;
            oNode = mMajorNode.SelectSingleNode(sName);
            if (oNode != null)
                mMajorNode.RemoveChild(oNode);

            oNode = null;
        }

        /// <summary>
        /// 设置子节点属性值
        /// </summary>
        /// <param name="sNodeKey"></param>
        /// <param name="sName"></param>
        /// <param name="sValue"></param>
        public void SetNodeAttribute(string sNodeKey, string sName, string sValue)
        {
            if (mMajorNode == null)
                return;

            System.Xml.XmlNode oNode;
            oNode = mMajorNode.SelectSingleNode(sNodeKey);
            if (oNode == null)
            {
                SetValue(sNodeKey, sNodeKey);
                oNode = mMajorNode.SelectSingleNode(sNodeKey);
            }
            if (oNode == null) return;

            System.Xml.XmlElement oElement;

            oElement = (XmlElement)oNode;
            oElement.SetAttribute(sName, sValue);
        }


        /// <summary>
        /// 读取子节点属性值
        /// </summary>
        /// <param name="sName"></param>
        /// <param name="sDefault_value"></param>
        /// <returns></returns>
        public string GetNodeAttribute(string sNodeKey, string sName, string sDefault_value = "")
        {
            if (mMajorNode == null)
                return sDefault_value;

            System.Xml.XmlNode oNode;
            oNode = mMajorNode.SelectSingleNode(sNodeKey);
            if (oNode == null)
            {
                return sDefault_value;
            }

            System.Xml.XmlElement oElement;
            oElement = (XmlElement)oNode;
            return string.Empty + oElement.GetAttribute(sName);
        }

        /// <summary>
        /// 删除子节点属性
        /// </summary>
        /// <param name="sName"></param>
        public void DeleteNodeAttribute(string sNodeKey, string sName)
        {
            if (mMajorNode == null)
                return;

            System.Xml.XmlNode oNode;
            oNode = mMajorNode.SelectSingleNode(sNodeKey);
            if (oNode == null)
            {
                return;
            }
            System.Xml.XmlElement oElement;
            oElement = (XmlElement)oNode;
            oElement.RemoveAttribute(sName);
        }




        /// <summary>
        /// 保存XML
        /// </summary>
        /// <param name="sPath"></param>
        /// <returns></returns>
        public bool Save(string sPath)
        {
            try
            {
                mXMLDoc.Save(sPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// 返回 XML 对象
        /// </summary>
        /// <returns></returns>
        public System.Xml.XmlDocument Document()
        {
            return mXMLDoc;
        }

        /// <summary>
        /// 返回 XML根节点
        /// </summary>
        /// <returns></returns>
        public System.Xml.XmlElement GetElement()
        {
            return mElement;
        }
        /// <summary>
        /// 返回 XML根节点
        /// </summary>
        /// <returns></returns>
        public System.Xml.XmlNode GetNode()
        {
            return mMajorNode;
        }


        /// <summary>
        /// 返回xml InnerXml
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return mXMLDoc.InnerXml;
        }
    }
}
