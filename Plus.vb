Public Class Plus
    Dim bdd As New actionBDD
    Dim ligne As Integer = 0
    Dim ensEntite As DataSet
    Dim nbreLigneMaxEntite As Integer
    Dim ensSignataires As DataSet
    Dim ligneSignataire As Integer = 0
    Dim nbreLigneMaxSignataire As Integer

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        bdd.enregistrerEntite(nomEntite.Text, typeEntite.Text, obtenirEntier(idParent.Text), typeParent.Text)
        chargerDataGridEntite()
    End Sub

    Private Sub idParent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles idParent.SelectedIndexChanged
        Dim idEntite As Integer
        idEntite = obtenirEntier(idParent.Text)
        typeParent.Text = bdd.obtenirTypeEntite(idEntite)
    End Sub

    Private Function obtenirEntier(chaine As String) As Integer
        Dim entier As Integer
        entier = CType(Trim(Split(chaine, "-")(1)), Integer)
        Return entier
    End Function

    Private Sub idParent_SelectedIndexGotFocus(sender As Object, e As EventArgs) Handles idParent.GotFocus
        Dim entites() As String = bdd.obtenirEntites()
        idParent.Items.Add("0")
        For Each i In entites
            If i <> vbNullString Then
                idParent.Items.Add(i)
            End If
        Next
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles signatureSignateur.Click
        With OpenFileDialog1
            .Title = "Choisir la signature"
            .ShowDialog()
            signatureSignateur.Image = Image.FromFile(.FileName)
        End With

    End Sub

    Private Sub deplacerFichier(lienFichier As String, nouveauLien As String)
        FileCopy(lienFichier, nouveauLien)
        'Dim lien As String = signatureSignateur.ImageLocation()
        'MsgBox(lienFichier)
        'FileCopy(
    End Sub

    Private Sub TabPage2_Click(sender As Object, e As EventArgs) Handles TabPage2.Click

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        ensEntite = bdd.chercherEntite(nomEntite.Text)
        nbreLigneMaxEntite = bdd.chercherEntite(nomEntite.Text).Tables("EntiteAcademique").Rows.Count
        chargerEntites()
    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        ligne = 0
        chargerEntites()
    End Sub

    Private Sub chargerEntites()
        If nbreLigneMaxEntite > 0 Then
            typeEntite.Text = ensEntite.Tables("EntiteAcademique").Rows(ligne).Item(1)
            idParent.Text = ensEntite.Tables("EntiteAcademique").Rows(ligne).Item(2)
            typeParent.Text = ensEntite.Tables("EntiteAcademique").Rows(ligne).Item(3)
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        If ligne > 0 Then
            ligne -= 1
        End If
        chargerEntites()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        If ligne < nbreLigneMaxEntite - 1 Then
            ligne += 1
        End If
        chargerEntites()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        ligne = nbreLigneMaxEntite - 1
        chargerEntites()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If nomSignataire.Text <> "" And titreSignateur.Text <> "" And OpenFileDialog1.FileName <> "OpenFileDialog1" Then
            bdd.enregistrerSignataires(nomSignataire.Text, titreSignateur.Text, True)

            Dim idSignataire As Integer = bdd.obtenirIdSignataire(nomSignataire.Text, titreSignateur.Text)
            'OpenFileDialog1.FileName
            Dim parties() As String = Split(OpenFileDialog1.FileName, ".")
            Dim extens As String = parties(parties.Count - 1)
            deplacerFichier(OpenFileDialog1.FileName, "img/signatures/" & idSignataire & ".signa")
            'MsgBox(idDentifiant)
            chargerDataGridSignataire()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ensSignataires = bdd.obtenirSignataire(nomSignataire.Text)
        nbreLigneMaxSignataire = bdd.obtenirSignataire(nomSignataire.Text).Tables("Signataire").Rows.Count
        chargerSignataires()
    End Sub

    Private Sub chargerSignataires()
        nomSignataire.Text = ensSignataires.Tables("Signataire").Rows(ligneSignataire).Item(1)
        titreSignateur.Text = ensSignataires.Tables("Signataire").Rows(ligneSignataire).Item(2)
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        If ligneSignataire > 0 Then
            ligneSignataire -= 1
        End If
        chargerSignataires()
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        If ligneSignataire < nbreLigneMaxSignataire - 1 Then
            ligneSignataire += 1
        End If
        chargerSignataires()
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ligne = 0
        chargerSignataires()
    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        ligne = nbreLigneMaxSignataire - 1
        chargerSignataires()
    End Sub

    Private Sub Plus_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub chargerDataGridSignataire()
        dataGridSignataires.DataSource = bdd.obtenirListeSignataires().Tables("Signataire")
    End Sub

    Private Sub chargerDataGridEntite()
        dataGridEntite.DataSource = bdd.obtenirListeSignataires().Tables("EntiteAcademique")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        chargerDataGridSignataire()
    End Sub
End Class