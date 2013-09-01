Imports Word = Microsoft.Office.Interop.Word
Public Class Accueil
    Dim oWord As Word.Application = CreateObject("Word.Application")
    Dim oDoc As Word.Document
    Dim leLien As String = "C:\users\athena\desktop\ficl.docx"
    Dim leLien1 As String = "C:\users\athena\desktop\ficl1.docx"

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        copierFichier()
        ouvrirFichier(leLien1)
        lireBookMarks()
        fermer()
    End Sub

    Private Sub copierFichier()
        FileCopy(leLien, leLien1)
    End Sub

    Private Sub ouvrirFichier(lien As String)
        oDoc = oWord.Documents.Open(leLien1)
        'oDoc = oWord.Documents.
    End Sub

    Private Sub lireBookMarks()
        For Each items In oDoc.Bookmarks
            items.range.text = "Eliel"
            'MsgBox(items.range.text)
        Next
        oDoc.Save()
    End Sub

    Private Sub fermer()
        oDoc.Close()
    End Sub

    Private Sub Accueil_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class