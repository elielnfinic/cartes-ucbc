Imports Word = Microsoft.Office.Interop.Word
Public Class Modele
    Dim oWord As Word.Application
    Dim oDoc As Word.Document
    Dim leLien As String = System.IO.Directory.GetCurrentDirectory & "\templates\modele.docx"
    Dim leLien1 As String = System.IO.Directory.GetCurrentDirectory & "\templates\modele1.docx"
    Dim lienModeleDonneesAppariteur As String = System.IO.Directory.GetCurrentDirectory & "\templates\modeleApp.docx"

    Public Function ouvrirTemplate() As Boolean
        copierFichier()
        ouvrirFichier(leLien1)
        obtenirMarques()
        fermer()
        Return True
    End Function

    Private Sub ouvrirFichier(lien As String)
        oWord = CreateObject("Word.Application")
        oDoc = oWord.Documents.Open(leLien1)

    End Sub



    Private Sub obtenirMarques()
        MsgBox("Les noms sont " & oDoc.Bookmarks.Item("noms").Range.Text)
        'modifierValeurs("01612", "Angemito Basebabua", "L4", "Genie informatique", "Congolaise", "Butembo", "20/06/1995", "Rigo", "Recteur", "Cool")

        ' For Each items In oDoc.Bookmarks
        'items.range.text = "Eliel"
        'MsgBox(items.range.text)
        ' MsgBox(items.ToString)

        ' Next
        oDoc.Save()
    End Sub

    Public Sub imprimer(carte As DonneesCarte)
        copierFichier()
        ouvrirFichier(leLien1)
        'MsgBox("Nom du signateur est " & carte.nomSignateur)
        modifierValeurs(carte.matricule, carte.noms, carte.auditoire, carte.departement, carte.nationalite, carte.lieuNaissance, carte.dateNaissance, carte.nomSignateur, carte.titreSignateur, carte.lienSignature, carte.lieuSignature, carte.dateSignature)
        oDoc.Save()
        fermer()
    End Sub

    Private Sub modifierValeurs(matricule As String, noms As String, auditoire As String, departement As String, nationalite As String, lieuNaissance As String, dateNaissance As String, nomSignateur As String, titreSignateur As String, imagSignature As String, lieuSignature As String, dateSignature As String)
        Dim dossierActuel = System.IO.Directory.GetCurrentDirectory
        oDoc.Bookmarks.Item("noms").Range.Text = noms
        oDoc.Bookmarks.Item("numeroMatricule").Range.Text = matricule
        oDoc.Bookmarks.Item("auditoire").Range.Text = auditoire
        oDoc.Bookmarks.Item("departement").Range.Text = departement
        oDoc.Bookmarks.Item("nationalite").Range.Text = nationalite
        oDoc.Bookmarks.Item("lieuNaissance").Range.Text = lieuNaissance
        oDoc.Bookmarks.Item("dateNaissance").Range.Text = dateNaissance
        oDoc.Bookmarks.Item("nomSignateur").Range.Text = nomSignateur
        oDoc.Bookmarks.Item("titreSignateur").Range.Text = titreSignateur

        'oDoc.Bookmarks.Item("finValidite").Range.Text = titreSignateur
        oDoc.Bookmarks.Item("lieuSignature").Range.Text = lieuSignature
        oDoc.Bookmarks.Item("dateSignature").Range.Text = dateSignature

        'oDoc.Bookmarks.Item("imagSignature").Range.Text = imagSignature
        'oDoc.Bookmarks.Item("photoPasseport").Range.Text = "img/photos/" & matricule & ".passport"
        oDoc.Bookmarks.Item("codeBarMatricule").Range.Text = "*" & matricule & "*"
        MsgBox(dossierActuel & "\img\photos\" & matricule & ".jpg")
        oDoc.Bookmarks.Item("photoPasseport").Range.InlineShapes.AddPicture(dossierActuel & "\img\photos\" & matricule & ".jpg") ' "C:\Users\Athena\Documents\Visual Studio 2012\Projects\Student\Student\bin\Debug\img\photos\1.jpg")
    End Sub


    Private Sub fermer()
        oDoc.PrintOut()
        oDoc.Close()
        oWord.Application.Quit()
    End Sub

    Private Sub copierFichier()
        FileCopy(leLien, leLien1)
    End Sub
End Class
