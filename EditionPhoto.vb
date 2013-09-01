Public Class EditionPhoto
    Public lienPhoto As String
    Public nomPhoto As String
    Public lienDossier As String

    Private Sub EditionPhoto_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'MsgBox("Lien de la photo est " & lienPhoto)
        '.ImageLocation = lienPhoto
        photo.Image = Image.FromFile(lienPhoto)
        afficherImg()
        bar1.Maximum = largeur
        bar2.Maximum = hauteur
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If positionX > 10 Then
            positionX -= 10
            bar1.Value = positionX
            bar2.Value = positionY
            afficherImg()
        End If
    End Sub

    Dim positionX As Integer = 0
    Dim positionY As Integer = 0
    Dim largeur, hauteur As Integer
    Private Sub afficherImg()
        Dim img As New Bitmap(lienPhoto)

        Dim g As Graphics
        g = Graphics.FromImage(img)
        largeur = img.Width
        hauteur = img.Height

        g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
        g.CompositingQuality = Drawing2D.CompositingQuality.HighQuality

        g.DrawImage(img, New Rectangle(0, 0, 333, 449), New Rectangle(positionY, positionX, 333, 449), GraphicsUnit.Pixel)
        photo.Image = img
    End Sub

    Public Function SavePhoto(ByVal src As Image, ByVal dest As String, ByVal w As Integer) As Boolean
        Dim imgTmp As System.Drawing.Image
        Dim imgFoto As System.Drawing.Bitmap

        imgTmp = src
        imgFoto = New System.Drawing.Bitmap(333, 449)
        Dim recDest As New Rectangle(0, 0, 333, 449)
        Dim gphCrop As Graphics = Graphics.FromImage(imgFoto)

        gphCrop.DrawImage(imgTmp, recDest, New Rectangle(0, 0, 333, 449), GraphicsUnit.Pixel)

        Dim myEncoder As System.Drawing.Imaging.Encoder
        Dim myEncoderParameter As System.Drawing.Imaging.EncoderParameter
        Dim myEncoderParameters As System.Drawing.Imaging.EncoderParameters

        Dim arrayICI() As System.Drawing.Imaging.ImageCodecInfo = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders()
        Dim jpegICI As System.Drawing.Imaging.ImageCodecInfo = Nothing
        Dim x As Integer = 0
        For x = 0 To arrayICI.Length - 1
            If (arrayICI(x).FormatDescription.Equals("JPEG")) Then
                jpegICI = arrayICI(x)
                Exit For
            End If
        Next
        myEncoder = System.Drawing.Imaging.Encoder.Quality
        myEncoderParameters = New System.Drawing.Imaging.EncoderParameters(1)
        myEncoderParameter = New System.Drawing.Imaging.EncoderParameter(myEncoder, 60L)
        myEncoderParameters.Param(0) = myEncoderParameter
        imgFoto.Save(dest, jpegICI, myEncoderParameters)
        imgFoto.Dispose()
        imgTmp.Dispose()

        Return True
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If positionY > 10 Then
            positionY -= 10
            bar1.Value = positionX
            bar2.Value = positionY
            afficherImg()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If positionY <= largeur Then
            positionY += 10
            bar1.Value = positionX
            bar2.Value = positionY
            afficherImg()
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If positionX <= hauteur Then
            positionX += 10
            bar1.Value = positionX
            bar2.Value = positionY
            afficherImg()
        End If
    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles bar1.Scroll
        positionY = bar1.Value
        afficherImg()
    End Sub

    Private Sub bar2_Scroll(sender As Object, e As EventArgs) Handles bar2.Scroll
        positionX = bar2.Value
        afficherImg()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        SavePhoto(photo.Image, lienDossier & "/" & nomPhoto & ".jpg", 333)
    End Sub
End Class