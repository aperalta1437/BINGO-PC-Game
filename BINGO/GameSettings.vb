Public Class GameSettings
    Private intChoice As Integer = 0
    Private frmMainForm As MainForm
    Private blnIsReadyToPlay As Boolean = False
    Private blnIsAnotherGame As Boolean

    Sub New(intGiven As Integer, frmGiven As MainForm, ByVal blnIsAnotherGame As Boolean)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        intChoice = intGiven
        frmMainForm = frmGiven
        Me.blnIsAnotherGame = blnIsAnotherGame

    End Sub


    Private Sub GameSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CenterForm(Me)

        Select Case intChoice
            Case 0
                Label1.ForeColor = Color.White
                Label2.ForeColor = Color.White
                Label3.ForeColor = Color.White
                Label4.ForeColor = Color.White
                Label5.ForeColor = Color.White
                Label6.ForeColor = Color.White
                Me.BackColor = Color.Black
            Case 1
                Label1.ForeColor = Color.Black
                Label2.ForeColor = Color.Black
                Label3.ForeColor = Color.Black
                Label4.ForeColor = Color.Black
                Label5.ForeColor = Color.Black
                Label6.ForeColor = Color.Black
                Me.BackColor = Color.White
            Case 2
                Label1.ForeColor = Color.White
                Label2.ForeColor = Color.White
                Label3.ForeColor = Color.White
                Label4.ForeColor = Color.White
                Label5.ForeColor = Color.White
                Label6.ForeColor = Color.White
                Me.BackColor = Color.DimGray
            Case 3
                Label1.ForeColor = Color.Black
                Label2.ForeColor = Color.Black
                Label3.ForeColor = Color.Black
                Label4.ForeColor = Color.Black
                Label5.ForeColor = Color.Black
                Label6.ForeColor = Color.Black
                Me.BackColor = Color.DimGray
        End Select
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' Close this form.
        Me.Close()
    End Sub

    Private Sub btnPlay_Click(sender As Object, e As EventArgs) Handles btnPlay.Click
        Try
            If (txtUsername.Text = "") OrElse (cboNumOfPlayers.Text = "") OrElse (cboNumOfBoards.Text = "") Then
                Throw New System.Exception("All requested information is required." & vbNewLine &
                                           "Please, provide all the information before continuing.")
            End If
            frmMainForm.Username = txtUsername.Text
            frmMainForm.NumOfPlayers = CInt(cboNumOfPlayers.Text)
            frmMainForm.NumOfBoards = CInt(cboNumOfBoards.Text)
            blnIsReadyToPlay = True
        Catch ex As InvalidCastException
            MessageBox.Show("*** Invalid values! ***" & vbNewLine &
                            "Please, provide numbers where requested.")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        If blnIsReadyToPlay Then
            frmMainForm.setGameTable()
            frmMainForm.chgMenuToBattlefield(blnIsAnotherGame)
            Me.Close()              ' Close the form.
        End If
    End Sub
End Class