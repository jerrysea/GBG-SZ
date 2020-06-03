Imports Newtonsoft.Json
Imports System.IO
Imports System.Xml
Imports System.Text

Public Class JsonXmlObjectParser
    ''' <summary>
    ''' Convert XML to JSON
    ''' </summary>
    ''' <param name="StrXml"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function XmlToJson(ByVal StrXml As String) As String
        Dim doc As New Xml.XmlDocument
        doc.LoadXml(StrXml)
        Dim json As String = JsonConvert.SerializeXmlNode(doc)
        Return json
    End Function
    ''' <summary>
    ''' Convert JSON to XML
    ''' </summary>
    ''' <param name="StrJson"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function JsonToXml(ByVal StrJson) As String
        Dim doc As Xml.XmlDocument
        doc = JsonConvert.DeserializeXmlNode(StrJson)
        Return ConvertXmlToString(doc)
    End Function
    ''' <summary>
    ''' Convert XMl Object TO String
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertXmlToString(ByVal doc As XmlDocument) As String

        Dim stream As New MemoryStream()
        Dim writer As New XmlTextWriter(stream, Nothing)
        writer.Formatting = System.Xml.Formatting.Indented

        doc.Save(writer)
        Dim sr As New StreamReader(stream, System.Text.Encoding.UTF8)
        stream.Position = 0
        Dim xmlString As String = sr.ReadToEnd()
        sr.Close()
        stream.Close()
        Return xmlString

    End Function

End Class
