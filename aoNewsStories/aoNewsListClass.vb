
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace Contensive.Addons.aoNewsStorys

    Public Class aoNewsListClass

        Inherits AddonBaseClass
        '
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String = ""
            '
            Try
                Dim header As String = ""
                Dim breif As String = ""
                Dim image As String = ""
                Dim story As String = ""
                Dim link As String = ""
                Dim sql As String = ""
                Dim layout As CPBlockBaseClass = CP.BlockNew
                Dim blockLayout As CPBlockBaseClass = CP.BlockNew
                Dim repeaterListHtml As String = ""
                Dim storyCnt As Integer = 0
                Dim histCount As Integer = 0
                Dim exitLoop As Boolean = False
                Dim storyID As Integer = CP.Doc.GetInteger("newsStoryId")
                Dim cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                Dim storyDate As Date = New Date(2000, 6, 9)
                Dim mydate As Date = Today
                Dim myMonth As String = mydate.ToString("MMMM")
                Dim Myday As Integer = mydate.Day
                Dim minDate As Date = New Date(2000, 6, 9)
                Dim tmp2 As String = layout.GetHtml()
                Dim brief As String
                Dim repeaterHtml As String
                Dim cachename As String = "News Stories Latest News"
                '
                layout.OpenLayout("aoNews Story Home Page News List")
                '
                returnHtml = CP.Cache.Read(cachename)
                If returnHtml = "" Then
                    If cs.Open("story List", "storydate<=" & CP.Db.EncodeSQLDate(Now), "storydate  Desc") Then
                        storyCnt = CP.Doc.GetInteger("numberOfStories")
                        If storyCnt = 0 Then
                            storyCnt = 3 'myStoryCount 
                        End If
                        exitLoop = False
                        Do
                            '
                            histCount += 1
                            header = cs.GetText("Name")
                            storyDate = cs.GetDate("storydate")
                            link = cs.GetText("link")
                            brief = cs.GetText("brief")
                            story = cs.GetText("story")
                            '
                            blockLayout.Load(layout.GetInner(".latestNewsUL"))
                            repeaterHtml = blockLayout.GetHtml()
                            '
                            If storyDate.ToString <> "" Then
                                myMonth = storyDate.ToString("MMMM")
                                Myday = storyDate.Day
                                '
                                blockLayout.SetInner(".art-mo", myMonth)
                                blockLayout.SetInner(".art-da", Myday)
                            End If
                            blockLayout.SetOuter(".art-title", "<p style=""padding-left:10px;"">" & brief & "</br>")
                            '
                            repeaterListHtml &= blockLayout.GetHtml
                            '
                            If storyCnt <> 0 Then
                                If histCount >= storyCnt Then
                                    exitLoop = True
                                End If
                            End If
                            '
                            Call cs.GoNext()
                        Loop While cs.OK And (Not exitLoop)
                    End If
                    Call cs.Close()
                    'do
                    ' read a table
                    ' end do

                    layout.SetInner(".latestNewsUL", repeaterListHtml)

                    returnHtml = layout.GetHtml
                    Call CP.Cache.Save(cachename, returnHtml, "story List", Now.AddDays(1).Date())
                End If
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in multiFormAjaxSample.execute")
            End Try
            Return returnHtml
        End Function

    End Class
End Namespace



