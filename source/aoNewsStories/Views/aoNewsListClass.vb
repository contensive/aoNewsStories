
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.Addons.aoNewsStories.Controllers
Imports Contensive.Addons.aoNewsStories.Models.Db

Imports Contensive.BaseClasses

Namespace Views

    Public Class aoNewsListClass
        Inherits AddonBaseClass
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Try
                Dim viewData As New NewsListViewModel()
                Using cs As BaseClasses.CPCSBaseClass = CP.CSNew()
                    Dim storyCnt As Integer = CP.Doc.GetInteger("numberOfStories")
                    If storyCnt = 0 Then
                        storyCnt = 3
                    End If
                    If cs.Open("story List", "storydate<=" & CP.Db.EncodeSQLDate(Now), "storydate  Desc") Then
                        Dim storyCount As Integer = 0
                        Do
                            Dim storyDate As Date = cs.GetDate("storydate")
                            viewData.newsList.Add(New NewsViewModel() With {
                                    .day = storyDate.Day,
                                    .month = storyDate.ToString("MMMM"),
                                    .title = cs.GetText("brief")
                                })
                            storyCount += 1
                            Call cs.GoNext()
                        Loop While cs.OK And (storyCount < storyCnt)
                    End If
                    Call cs.Close()
                End Using
                Return Nustache.Core.Render.StringToString(My.Resources.NewsStoryHomePageNewsList, viewData)
            Catch ex As Exception
                CP.Site.ErrorReport(ex, "error in multiFormAjaxSample.execute")
                Return String.Empty
            End Try
        End Function
    End Class
    '
    Public Class NewsListViewModel
        Public newsList As New List(Of NewsViewModel)
    End Class
    '
    Public Class NewsViewModel
        Public month As String
        Public day As String
        Public title As String
    End Class
    '
End Namespace






