Public Class Principal
    Dim bdd As New actionBDD
    Dim dernierId As String
    Dim signataireEnCours As String
    Dim leWebCam As New WebCam
    Dim dossierActuel As String
    Dim ensEtudiants As DataSet
    Dim ligneEtudiants As Integer = 0
    Dim nbreEtudiantsMax As Integer = 0
    Dim idEtudiantEnCours As String

    Private Sub TabPage1_Click(sender As Object, e As EventArgs) Handles TabPage1.Click

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem2.Click

    End Sub

    Private Sub OutilsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OutilsToolStripMenuItem.Click

    End Sub

    Private Sub decouper(matricule As String)
        'MsgBox(matricule(1))
        Dim part1 As String
        Dim part2 As String
        part1 = matricule(0) & matricule(1) & matricule(2)
        part2 = matricule(3) & matricule(4)

        Dim ent1, ent2 As Integer
        ent1 = CType(part1, Integer)
        ent2 = CType(part2, Integer)

        MsgBox("La premiere partie est " & ent1 & ", deuxieme partie est " & ent2)
    End Sub

    Private Function obtenirGrandMatricule(matricule1 As String, matricule2 As String) As String
        Dim part1 As String
        Dim part2 As String
        part1 = matricule1(0) & matricule1(1) & matricule1(2)
        part2 = matricule1(3) & matricule1(4)
        Dim ent1, ent2, ent3, ent4 As Integer
        ent1 = CType(part1, Integer)
        ent2 = CType(part2, Integer)
        part1 = matricule2(0) & matricule2(1) & matricule2(2)
        part2 = matricule2(3) & matricule2(4)
        ent3 = CType(part1, Integer)
        ent4 = CType(part2, Integer)

        If ent4 > ent2 Then
            'Le deuxieme est grand
            Return matricule2
        ElseIf ent4 < ent2 Then
            'Le deuxieme est petit
            Return matricule1
        ElseIf ent4 = ent2 Then
            If ent1 > ent3 Then
                'Le premier est grand
                Return matricule1
            Else
                'Le deuxieme est grand
                Return matricule2
            End If
        End If
        Return vbNull
    End Function

    Private Sub Principal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'InscriptionDataSet.sInscrit' table. You can move, or remove it, as needed.
        'MsgBox(obtenirGrandMatricule("11612", "01612"))
        obtenirExcel()
        setDateExpiration()
        Me.SInscritTableAdapter.Fill(Me.InscriptionDataSet.sInscrit)
        dossierActuel = System.IO.Directory.GetCurrentDirectory
        'MsgBox("0x".ToString.PadLeft(5, "u"))
        ' Date.Now().Day
        GroupBox2.Visible = False
        GroupBox6.Visible = False
        GroupBox7.Visible = False

        txtMatricule.Text = bdd.obtenirDernierMatricule
        lancerCapture.Visible = False
        chargerListeEtudiantsAImprimer()
        cacherEditionInscrire()
    End Sub

    Private Sub obtenirExcel()
        'Dim excel As New Excel
        'ex.obtenirDonnees()
    End Sub

    Private Sub reinitialiserChampInscrire()
        txtMatricule.Text = ""
        txtNom.Text = ""
        txtPostNom.Text = ""
        txtPrenom.Text = ""
        txtLieuNaissance.Text = ""
        cbxNationalite.Text = ""
        cbxHouse.Text = ""
    End Sub

    Private Sub cacherEditionInscrire()
        Button7.Visible = False
        Button8.Visible = False
    End Sub

    Private Sub montrerEditionInscrire()
        Button7.Visible = True
        Button8.Visible = True
    End Sub

    Private Sub chargerListeEtudiantsAImprimer()
        'bdd.chargerListeEtudiantsImprimer()
        Dim ensDonnees As DataSet = bdd.chargerListeEtudiantsImprimer(1)
        DataGridView2.DataSource = ensDonnees.Tables("Etudiants")


    End Sub



    Private Sub setDateExpiration()
        dateExpiration.Value = New Date(CType(Now.Year, Integer) + 1, Now.Month, Now.Day)
    End Sub

    Private Sub constituerAutreId()
        dernierId = bdd.obtenirDernierId

    End Sub


    Private Sub chargerNationalites()
        cbxNationalite.Items.Clear()
        Dim elts() As String = bdd.obtenirNationalite()
        For Each elt In elts
            If elt <> vbNullString Then
                cbxNationalite.Items.Add(elt)
            End If
        Next
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'enregistrerImgWebCam(txtMatricule.Text)
        If txtMatricule.Text = "" Then
            afficherErreur("Le numero matricule vide!")
        ElseIf txtNom.Text = "" Then
            afficherErreur("Le nom est vide!")
        ElseIf txtPostNom.Text = "" Then
            afficherErreur("Le post-nom est vide!")
        ElseIf txtPrenom.Text = "" Then
            afficherErreur("Le prenom est vide!")
        ElseIf txtLieuNaissance.Text = "" Then
            afficherErreur("Le lieu de naissance est vide!")
        ElseIf dateNaissance.Text = "" Then
            afficherErreur("La date de naissance vide!")
        ElseIf cbxNationalite.Text = "" Then
            afficherErreur("La nationalite est vide!")
        ElseIf cbxHouse.Text = "" Then
            afficherErreur("Le house est vide!")
        Else
            bdd.enregistrerEtudiant(txtMatricule.Text, txtNom.Text, txtPostNom.Text, txtPrenom.Text, txtLieuNaissance.Text, cbxNationalite.Text, dateNaissance, cbxHouse.Text)
            enregistrerImgWebCam(txtMatricule.Text)
            reinitialiserChampInscrire()
        End If
    End Sub

    Private Sub cbxNationalite_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxNationalite.Click
        chargerNationalites()
    End Sub

    Private Sub afficherErreur(erreur As String)
        afficheurErreur.Text = "Numero matricule "
        afficheurErreur.BackColor = Color.White
        afficheurErreur.ForeColor = Color.Red
    End Sub

    Private Sub enleverAffichageErreur()
        afficheurErreur.Text = " "
        afficheurErreur.ForeColor = Color.Black
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbxMatricule.GotFocus
        Dim mat() As String = bdd.obtenirMatricules
        cbxMatricule.Items.Clear()
        For Each i In mat
            If i <> vbNullString Then
                cbxMatricule.Items.Add(i)
            End If
        Next
    End Sub

    Private Sub SignatairesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SignatairesToolStripMenuItem.Click
        Plus.Show()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        If matriculeAffecter.Text <> "" And entiteAffecter.Text <> "" And AnneeAcademiqueAffecter.Text <> "" And provenanceAffecter.Text <> "" And pourcentageAffecter.Text <> "" Then
            Dim idEntite As Integer
            Dim tab() As String = Split(entiteAffecter.Text, "-")
            idEntite = CType(tab(2), Integer)
            bdd.enregistrerAffecterSalle(matriculeAffecter.Text, idEntite, AnneeAcademiqueAffecter.Text, provenanceAffecter.Text, pourcentageAffecter.Text)
        Else
            'Un des champs est vide
        End If
    End Sub

    Private Sub matriculeAffecter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles matriculeAffecter.GotFocus
        Dim elts As String() = bdd.obtenirMatricules()
        matriculeAffecter.Items.Clear()
        For Each elt In elts
            If elt <> vbNullString Then
                matriculeAffecter.Items.Add(elt)
            End If
        Next


    End Sub

    Private Sub entiteAffecter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles entiteAffecter.GotFocus
        Dim elts As String() = bdd.obtenirEntitesEtParent()
        entiteAffecter.Items.Clear()
        For Each elt In elts
            If elt <> vbNullString Then
                entiteAffecter.Items.Add(elt)
            End If
        Next
    End Sub

    Private Sub entiteAffecter_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles entiteAffecter.SelectedIndexChanged

    End Sub

    Private Sub AnneeAcademiqueAffecter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles AnneeAcademiqueAffecter.GotFocus
        Dim elts As String() = bdd.obtenirAnneeAcademiques()
        AnneeAcademiqueAffecter.Items.Clear()
        For Each elt In elts
            If elt <> vbNullString Then
                AnneeAcademiqueAffecter.Items.Add(elt)
            End If
        Next
    End Sub

    Private Sub provenanceAffecter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles provenanceAffecter.GotFocus
        If matriculeAffecter.Text <> "" Then
            Dim derniereClasse = bdd.obtenirDerniereClassse(matriculeAffecter.Text)
            If derniereClasse <> vbNullString Then
                ' MsgBox(bdd.obtenirDerniereClassse("01612"))
                provenanceAffecter.Text = derniereClasse
            End If
        End If
    End Sub

    Private Sub provenanceAffecter_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles provenanceAffecter.SelectedIndexChanged

    End Sub

    Private Sub ComboBox3_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles signatairesImprimer.GotFocus
        'MsgBox("Recherche des signataires")
        Dim elts() As String = bdd.obtenirSignataires()
        ' MsgBox("Les gens sont " & elts.Count)
        signatairesImprimer.Items.Clear()
        For Each elt In elts
            ' MsgBox(elt)
            If elt <> vbNullString Then
                signatairesImprimer.Items.Add(elt)
            End If
        Next

    End Sub

    Private Sub ComboBox3_SelectedIndexChanged_2(sender As Object, e As EventArgs) Handles signatairesImprimer.SelectedIndexChanged

    End Sub


    Private Sub prendrePhoto()
        leWebCam.Start()
    End Sub

    Private Sub finirPrendrePhoto()
        leWebCam.Stop()
    End Sub

    Private Sub enregistrerImgWebCam(nomImage As String)
        ' MsgBox("Enregistrement de " & nomImage)
        Try
            prisePasseport.Image.Save("img/photos/" & nomImage & ".JPG")
        Catch ex As Exception
        End Try
        ' MsgBox("Enregistrement image termine")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles capturerPhoto.Click
        leWebCam = New WebCam()
        leWebCam.InitializeWebCam(prisePasseport)
        prendrePhoto()
        lancerCapture.Visible = True
    End Sub

    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        lancerCapture.Visible = False
        With OpenFileDialog1
            .Title = "Choisir la signature"
            .ShowDialog()
            prisePasseport.Image = Image.FromFile(.FileName)
        End With
    End Sub

    Private Sub Process1_Exited(sender As Object, e As EventArgs)

    End Sub

    Private Sub Button13_Click(sender As Object, e As EventArgs)
        'prisePasseport.Image.RotateFlip(RotateFlipType.Rotate270FlipY)
    End Sub

    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles lancerCapture.Click
        leWebCam.Stop()
        lancerCapture.Visible = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'MsgBox("Chargement ...")
        'Dim lesSignataires = signatairesImprimer.Text
        Dim dateSignature As String
        dateSignature = Date.Now.Day & "/" & Date.Now.Month & "/" & Date.Now.Year

        Dim tab() As String = Split(signatairesImprimer.Text, "-")
        Dim idSignataire = Trim(tab(2))
        Dim carte As DonneesCarte = bdd.obtenirDonneesCarte(cbxMatricule.Text, idSignataire)
        ' MsgBox(carte.departement)
        'Exit Sub
        carte.dateSignature = dateSignature
        carte.lieuSignature = cbxLieuImpression.Text
        Dim modeleCarte As New Modele
        'For Each item In modeleCarte
        'item = New Modele
        modeleCarte.imprimer(carte)
        'Next
        ' Dim x As New Modele
        ' x.ouvrirTemplate()
        'MsgBox("Ouverture du template avec succes!")
    End Sub
    Private Sub Button4_Click_1(sender As Object, e As EventArgs) Handles Button4.Click
        If txtMatricule.Text <> "" Then
            enregistrerImgWebCam(txtMatricule.Text & ".raw")
            EditionPhoto.lienPhoto = dossierActuel & "\img\photos\" & txtMatricule.Text & ".raw.JPG"
            EditionPhoto.nomPhoto = txtMatricule.Text
            EditionPhoto.lienDossier = dossierActuel & "\img\photos\"
            EditionPhoto.Show()
        End If
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As PaintEventArgs) Handles prisePasseport.Paint

    End Sub

    Private Sub AppariteurToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'Donnees.Show()
    End Sub

    Private Sub EntitésAcadémiquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EntitésAcadémiquesToolStripMenuItem.Click
        Plus.Show()
    End Sub

    Private Sub Button10_Click_1(sender As Object, e As EventArgs) Handles Button10.Click
        ensEtudiants = bdd.chercherEtudiant(txtNom.Text)
        nbreEtudiantsMax = ensEtudiants.Tables("Etudiants").Rows.Count
        chargerEtudiant()
    End Sub

    Private Sub chargerEtudiant()
        If ensEtudiants.Tables("Etudiants").Rows.Count > 0 Then
            txtMatricule.Text = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(1)
            txtNom.Text = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(2)
            txtPostNom.Text = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(3)
            txtPrenom.Text = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(4)
            txtLieuNaissance.Text = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(5)
            dateNaissance.Value = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(6)
            cbxNationalite.Text = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(9)
            cbxHouse.Text = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(8)
            idEtudiantEnCours = ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(0)
            GroupBox7.Visible = True
            chargerPhoto(dossierActuel & "\img\photos\" & ensEtudiants.Tables("Etudiants").Rows(ligneEtudiants).Item(1) & ".jpg", prisePasseport)
            If nbreEtudiantsMax > 1 Then
                GroupBox6.Visible = True
            End If
        End If


        'Chargement de la photo
    End Sub

    Private Sub chargerPhoto(lien As String, destination As PictureBox)
        If System.IO.File.Exists(lien) Then
            destination.Image = Image.FromFile(lien)
        Else
            destination.Image = Image.FromFile(dossierActuel & "\img\photos\defaut.png")
        End If
    End Sub

    Private Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        ligneEtudiants = nbreEtudiantsMax - 1
        chargerEtudiant()
    End Sub

    Private Sub Button13_Click_1(sender As Object, e As EventArgs) Handles Button13.Click
        ligneEtudiants = 0
        chargerEtudiant()
    End Sub

    Private Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        If ligneEtudiants > 0 Then
            ligneEtudiants -= 1
        End If
        chargerEtudiant()
    End Sub

    Private Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        If ligneEtudiants < nbreEtudiantsMax - 1 Then
            ligneEtudiants += 1
            chargerEtudiant()
        End If

    End Sub

    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If idEtudiantEnCours = "" Or idEtudiantEnCours = vbNullString Then
            afficherErreur("Impossible de trouver cet element!")
        ElseIf txtMatricule.Text = "" Then
            afficherErreur("Le numero matricule vide!")
        ElseIf txtNom.Text = "" Then
            afficherErreur("Le nom est vide!")
        ElseIf txtPostNom.Text = "" Then
            afficherErreur("Le post-nom est vide!")
        ElseIf txtPrenom.Text = "" Then
            afficherErreur("Le prenom est vide!")
        ElseIf txtLieuNaissance.Text = "" Then
            afficherErreur("Le lieu de naissance est vide!")
        ElseIf dateNaissance.Text = "" Then
            afficherErreur("La date de naissance vide!")
        ElseIf cbxNationalite.Text = "" Then
            afficherErreur("La nationalite est vide!")
        ElseIf cbxHouse.Text = "" Then
            afficherErreur("Le house est vide!")
        Else
            bdd.enregistrerEtudiant(txtMatricule.Text, txtNom.Text, txtPostNom.Text, txtPrenom.Text, txtLieuNaissance.Text, cbxNationalite.Text, dateNaissance, cbxHouse.Text)
            enregistrerImgWebCam(txtMatricule.Text)
            reinitialiserChampInscrire()
        End If
    End Sub

    Private Sub TabPage4_Click(sender As Object, e As EventArgs) Handles TabPage4.Click

    End Sub

    Dim matriculeContactEnCours As String = ""

    Private Sub Button17_Click(sender As Object, e As EventArgs) Handles Button17.Click
        If matriculeContact.Text = "" Then
            afficherErreur("Le numero matricule ne peut être vide!")
        Else
            Dim donnees() As String = bdd.obtenirIdentiteEtudiant(matriculeContact.Text)
            If donnees(1) <> matriculeContact.Text Then

            Else
                GroupBox2.Visible = True
                GroupBox2.Text = "Contact de " & donnees(2) & " " & donnees(3)
                DataGridView3.DataSource = bdd.obtenirContactsEtudiant(matriculeContact.Text).Tables("Contact")
                matriculeContactEnCours = donnees(1)
                'MsgBox(matriculeContactEnCours)
            End If
        End If
    End Sub

    Private Sub Button18_Click(sender As Object, e As EventArgs) Handles Button18.Click
        If matriculeContactEnCours = "" Then
        Else
            If typeContact.Text <> "" And valeurContact.Text <> "" Then
                Dim x As Boolean = bdd.ajouterContact(matriculeContactEnCours, typeContact.Text, valeurContact.Text)
                If x Then
                    DataGridView3.DataSource = bdd.obtenirContactsEtudiant(matriculeContact.Text).Tables("Contact")
                Else
                    MsgBox("Echec d'enregistrement")
                End If
            End If
        End If
    End Sub

    Private Sub DataGridView3_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView3.CellClick
        Dim x As Integer = DataGridView3.GetCellCount(DataGridViewElementStates.Selected)
        If x > 0 Then
            ' MsgBox(DataGridView3.SelectedCells(0).RowIndex)
            Dim elt As String
            elt = DataGridView3.Item(0, DataGridView3.SelectedCells(0).RowIndex).Value
            obtenirDonneesContact(elt)
        Else
            'Vide!
        End If
    End Sub

    Dim idContact As String = ""
    Private Sub obtenirDonneesContact(idContact As String)
        If idContact <> "" Then
            Me.idContact = idContact
            Dim x As DataSet = bdd.obtenirDonneesContacts(idContact)
            typeContactModifier.Text = x.Tables("Contact").Rows(0).Item(0)
            valeurContactModifier.Text = x.Tables("Contact").Rows(0).Item(1)
        End If
    End Sub

    Private Sub DataGridView3_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView3.CellContentClick

    End Sub

    Private Sub Button19_Click(sender As Object, e As EventArgs) Handles Button19.Click
        If Me.idContact <> "" And matriculeContactEnCours <> "" And typeContactModifier.Text <> "" And valeurContactModifier.Text <> "" Then
            bdd.mettreAJourContact(Me.idContact, matriculeContactEnCours, typeContactModifier.Text, valeurContactModifier.Text)
        End If
    End Sub

    Private Sub Button20_Click(sender As Object, e As EventArgs) Handles Button20.Click
        If Me.idContact <> "" Then
            Dim x As Boolean = bdd.supprimerContact(idContact)
            If x Then
                idContact = ""
            Else
                'Echec de suppression
            End If
        End If
    End Sub


    Private Sub Button22_Click(sender As Object, e As EventArgs) Handles Button22.Click

    End Sub

    Private Sub Button23_Click(sender As Object, e As EventArgs) Handles Button23.Click
        txtMatricule.Text = bdd.obtenirDernierMatricule
    End Sub

    Private Sub txtMatricule_TextChanged(sender As Object, e As EventArgs) Handles txtMatricule.GotFocus
        txtMatricule.Text = bdd.obtenirDernierMatricule
    End Sub

    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        If txtMatricule.Text Then
            Dim x As Boolean = bdd.supprimerEtudiant(txtMatricule.Text)
            If x Then
                MsgBox("Suppression avec succès!")
                reinitialiserChampInscrire()
            Else
                MsgBox("Echec de suppression!")
            End If
        End If
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)

    End Sub


    Private Sub DataGridView2_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellContentClick

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        'MsgBox("Tres bien")
        Dim x As Integer = DataGridView1.GetCellCount(DataGridViewElementStates.Selected)
        If x > 0 Then
            ' MsgBox(DataGridView3.SelectedCells(0).RowIndex)
            Dim elt As String
            elt = DataGridView1.Item(0, DataGridView1.SelectedCells(0).RowIndex).Value
            'MsgBox(elt)
            'obtenirDonneesContact(elt)
            montrerEditionInscrire()
            matriculeAffecter.Text = DataGridView1.Item(0, DataGridView1.SelectedCells(0).RowIndex).Value
            entiteAffecter.Text = DataGridView1.Item(1, DataGridView1.SelectedCells(0).RowIndex).Value
            AnneeAcademiqueAffecter.Text = DataGridView1.Item(5, DataGridView1.SelectedCells(0).RowIndex).Value
            provenanceAffecter.Text = DataGridView1.Item(4, DataGridView1.SelectedCells(0).RowIndex).Value
            pourcentageAffecter.Text = DataGridView1.Item(3, DataGridView1.SelectedCells(0).RowIndex).Value

        Else
            'Vide!
        End If
    End Sub

    Private Sub DataGridView1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class