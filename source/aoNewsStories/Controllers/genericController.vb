Option Explicit On
Option Strict On

Imports Contensive.BaseClasses

Namespace Controllers
    Public NotInheritable Class genericController
        Private Sub New()
        End Sub
        '
        '====================================================================================================
        ''' <summary>
        ''' if date is invalid, set to minValue
        ''' </summary>
        ''' <param name="srcDate"></param>
        ''' <returns></returns>
        Public Shared Function encodeMinDate(srcDate As DateTime) As DateTime
            Dim returnDate As DateTime = srcDate
            If srcDate < New DateTime(1900, 1, 1) Then
                returnDate = DateTime.MinValue
            End If
            Return returnDate
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' if valid date, return the short date, else return blank string 
        ''' </summary>
        ''' <param name="srcDate"></param>
        ''' <returns></returns>
        Public Shared Function getShortDateString(srcDate As DateTime) As String
            Dim returnString As String = ""
            Dim workingDate As DateTime = encodeMinDate(srcDate)
            If Not isDateEmpty(srcDate) Then
                returnString = workingDate.ToShortDateString()
            End If
            Return returnString
        End Function
        '
        '====================================================================================================
        Public Shared Function isDateEmpty(srcDate As DateTime) As Boolean
            Return (srcDate < New DateTime(1900, 1, 1))
        End Function
        '
        '====================================================================================================
        Public Shared Function getSortOrderFromInteger(id As Integer) As String
            Return id.ToString().PadLeft(7, "0"c)
        End Function
        '
        '====================================================================================================
        Public Shared Function getDateForHtmlInput(source As DateTime) As String
            If isDateEmpty(source) Then
                Return ""
            Else
                Return source.Year.ToString() + "-" + source.Month.ToString().PadLeft(2, "0"c) + "-" + source.Day.ToString().PadLeft(2, "0"c)
            End If
        End Function
        '
        '====================================================================================================
        Public Shared Function convertToDosPath(sourcePath As String) As String
            Return sourcePath.Replace("/", "\")
        End Function
        '
        '====================================================================================================
        Public Shared Function convertToUnixPath(sourcePath As String) As String
            Return sourcePath.Replace("\", "/")
        End Function
        '
        ' ====================================================================================================
        ''' <summary>
        ''' execute an addon within a column
        ''' </summary>
        ''' <param name="cp"></param>
        ''' <param name="addonId"></param>
        ''' <param name="columnPtr"></param>
        ''' <param name="instanceId"></param>
        ''' <returns></returns>
        Public Shared Function executeColumnAddon(cp As CPBaseClass, addonId As Integer, columnPtr As Integer, instanceId As String) As String
            If (addonId <= 0) Then Return "<!-- column-" & columnPtr.ToString() & " -->"
            'If (addonId <= 0) Then Return If(Not cp.User.IsEditingAnything, String.Empty, "<p style=""text-align:center;padding:10px;"">No Addon Selected</p>")
            cp.Doc.SetProperty("instanceId", instanceId & "-column:" & columnPtr)
            Return cp.Utils.ExecuteAddon(addonId.ToString)
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' buffer an url to include protocol
        ''' </summary>
        ''' <param name="url">Url that needs to nbe normalized</param>
        ''' <returns>If blank is returned, </returns>
        Public Shared Function verifyProtocol(url As String) As String
            '
            ' -- allow empty
            If (String.IsNullOrWhiteSpace(url)) Then Return String.Empty
            '
            ' -- allow /myPage
            If (url.Substring(0, 1) = "/") Then Return url
            '
            ' -- allow if it includes ://
            If (Not url.IndexOf("://").Equals(-1)) Then Return url
            '
            ' -- add http://
            Return "http://" & url
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' convert string into a style "height: {styleHeight};", if value is numeric it adds "px"
        ''' </summary>
        ''' <param name="styleheight"></param>
        ''' <returns></returns>
        Public Shared Function encodeStyleHeight(styleheight As String) As String
            Return If(String.IsNullOrWhiteSpace(styleheight), String.Empty, "overflow:hidden;height:" & styleheight & If(IsNumeric(styleheight), "px", String.Empty) & ";")
        End Function
        '
        '====================================================================================================
        ''' <summary>
        ''' convert string into a style "background-image: url(backgroundImage)
        ''' </summary>
        ''' <param name="backgroundImage"></param>
        ''' <returns></returns>
        Public Shared Function encodeStyleBackgroundImage(cp As CPBaseClass, backgroundImage As String) As String
            Return If(String.IsNullOrWhiteSpace(backgroundImage), String.Empty, "background-image: url(" & cp.Site.FilePath & backgroundImage & ");")
        End Function
        ''
        ''====================================================================================================
        '''' <summary>
        '''' if in editing mode, return an edit wrapper. If not, just return the innerHtml
        '''' </summary>
        '''' <param name="cp"></param>
        '''' <param name="innerHtml"></param>
        '''' <param name="instance"></param>
        '''' <param name="contentName"></param>
        '''' <returns></returns>
        'Public Shared Function addEditWrapper(cp As CPBaseClass, innerHtml As String, instance As DbBaseModel, contentName As String, designBlockCaption As String) As String
        '    If (Not cp.User.IsEditingAnything()) Then Return innerHtml
        '    Dim editHeader As String = cp.Html.div(cp.Content.GetEditLink(contentName, instance.id.ToString(), False, instance.name, True) & "&nbsp;" & designBlockCaption, "", "dbEditHeader")
        '    Return cp.Html.div(editHeader & innerHtml, "", "ccEditWrapper")
        'End Function
        '
        'Public Shared Function addEditWrapper(ByVal cp As CPBaseClass, ByVal innerHtml As String, ByVal instanceId As Integer, ByVal instanceName As String, ByVal contentName As String, ByVal designBlockCaption As String) As String
        '    If (Not cp.User.IsEditingAnything) Then Return innerHtml
        '    Dim editLink As String = getEditLink(cp, contentName, instanceId, designBlockCaption)
        '    Dim settingContent As String = cp.Html.div(innerHtml, "", "dbSettingWrapper")
        '    Dim settingHeader As String = cp.Html.div(editLink, "", "dbSettingHeader")
        '    Return cp.Html.div(settingHeader + settingContent)
        'End Function
        '
        '<div class="ccEditWrapper">
        '   <div Class="ccRecordLinkCon">
        '       <a href= "#" Class="ccRecordEditLink">asdf</a>
        '       <div Class="ccEditLinkEndCap">&nbsp;</div>
        '   </div>
        '   <div>content</div>
        '</div>

        ''
        ''
        ''
        'Public Shared Function getEditLink(ByVal cp As CPBaseClass, ByVal contentName As String, ByVal recordId As Integer, Caption As String) As String
        '    Return cp.Content.GetEditLink(contentName, recordId.ToString(), False, Caption, True)
        '    'Dim contentId As Integer = cp.Content.GetID(contentName)
        '    'If contentId = 0 Then Return String.Empty
        '    'Return "<a href=""/admin?af=4&aa=2&ad=1&cid=" & contentId & "&id=" & recordId & """ class=""ccRecordEditLink""><span style=""color:#0c0""><i title=""edit"" class=""fas fa-cog""></i></span>&nbsp;" & Caption & "</a>"
        'End Function
        '
        'Public Shared Function addEditWrapper(ByVal cp As CPBaseClass, ByVal innerHtml As String, ByVal recordId As Integer, ByVal contentName As String, ByVal caption As String) As String
        '    If (Not cp.User.IsEditingAnything) Then Return innerHtml
        '    Dim header As String = cp.Content.GetEditLink(contentName, recordId.ToString(), False, caption, True)
        '    Dim content As String = cp.Html.div(innerHtml, "", "")
        '    Return cp.Html.div(header + content, "", "ccEditWrapper")
        'End Function

        Public Shared Function addEditWrapper(ByVal cp As CPBaseClass, ByVal innerHtml As String, ByVal recordId As Integer, ByVal recordName As String, ByVal contentName As String) As String
            If (Not cp.User.IsEditingAnything) Then Return innerHtml
            Dim header As String = cp.Content.GetEditLink(contentName, recordId.ToString(), False, recordName, True)
            Dim content As String = cp.Html.div(innerHtml, "", "")
            Return cp.Html.div(header + content, "", "ccEditWrapper")
        End Function
    End Class
End Namespace

