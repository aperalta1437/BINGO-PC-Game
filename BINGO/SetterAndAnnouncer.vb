Public Class SetterAndAnnouncer
    Private intSquaresNums(,,,) As Integer
    Private blnMarkedSquares(,,,) As Boolean
    Private rand As New Random

    Private intCalledNums As New List(Of Integer)

    Private intNumOfPlayers As Integer
    Private intNumOfBoards As Integer

    Private intAnnouncedNum As Integer = 0
    Private intColumnNum As Integer

    Private blnIsGameOver As Boolean = False
    'Private blnUserHasAnnouncedNum As Boolean = False

    Public Sub New(ByVal intNumOfPlayers As Integer, ByVal intNumOfBoards As Integer)
        Dim intSquarePerBoardPerPerson(intNumOfPlayers, (intNumOfBoards - 1), 4, 4) As Integer
        Dim blnSquarePerBoardPerPerson(intNumOfPlayers, (intNumOfBoards - 1), 4, 4) As Boolean
        Dim intCurrSquareNum As Integer = 0         ' Declared and initialized here to increase performance.
        Dim intCurrColumnNums(4) As Integer         ' Declared and initialized here to increase performance.
        Dim blnIsRepeatedNum As Boolean = False
        Me.intNumOfPlayers = intNumOfPlayers
        Me.intNumOfBoards = intNumOfBoards

        For index1 As Integer = 0 To intNumOfPlayers                ' For each player.
            For index2 As Integer = 0 To (intNumOfBoards - 1)       ' For each player's board.
                For index3 As Integer = 0 To 4                      ' For each player's board's column.
                    For index4 As Integer = 0 To 4                  ' For each player's board's row.
                        If index3 = 2 AndAlso index4 = 2 Then
                            blnSquarePerBoardPerPerson(index1, index2, index4, index3) = True
                            Continue For
                        End If
                        Do
                            blnIsRepeatedNum = False
                            intCurrSquareNum = getSquareNum(index3)
                            For Each intVal As Integer In intCurrColumnNums
                                If intCurrSquareNum = intVal Then
                                    blnIsRepeatedNum = True
                                End If
                            Next
                        Loop While blnIsRepeatedNum

                        intCurrColumnNums(index4) = intCurrSquareNum
                        intSquarePerBoardPerPerson(index1, index2, index4, index3) = intCurrSquareNum
                    Next
                Next
            Next
        Next
        intSquaresNums = intSquarePerBoardPerPerson
        blnMarkedSquares = blnSquarePerBoardPerPerson
    End Sub

    Public Function getBoardNums(ByVal intPlayerNum As Integer, ByVal intBoardNum As Integer) As Integer(,)
        Dim intReturnedArray(4, 4) As Integer

        For index1 As Integer = 0 To 4
            For index2 As Integer = 0 To 4
                intReturnedArray(index1, index2) = intSquaresNums((intPlayerNum - 1), (intBoardNum - 1), index1, index2)
            Next
        Next

        Return intReturnedArray
    End Function

    Public Function getBoardMarks(ByVal intPlayerNum As Integer, ByVal intBoardNum As Integer) As Boolean(,)
        Dim blnReturnedArray(4, 4) As Boolean

        For index1 As Integer = 0 To 4
            For index2 As Integer = 0 To 4
                blnReturnedArray(index1, index2) = blnMarkedSquares((intPlayerNum - 1), (intBoardNum - 1), index1, index2)
            Next
        Next

        Return blnReturnedArray
    End Function


    Private Function getSquareNum(ByVal intColumnNum As Integer) As Integer
        Select Case intColumnNum
            Case 0
                Return rand.Next(1, 16)
            Case 1
                Return rand.Next(16, 31)
            Case 2
                Return rand.Next(31, 46)
            Case 3
                Return rand.Next(46, 61)
            Case 4
                Return rand.Next(61, 76)
        End Select
    End Function

    Public Function getNextNumber() As Integer
        Dim blnIsRepeated As Boolean

        Do
            blnIsRepeated = False
            intColumnNum = rand.Next(5)
            intAnnouncedNum = getSquareNum(intColumnNum)
            For Each intVal As Integer In intCalledNums
                If intVal = intAnnouncedNum Then
                    blnIsRepeated = True
                End If
            Next
        Loop While blnIsRepeated
        intCalledNums.Add(intAnnouncedNum)
        chkBoardsForNum(intColumnNum)

        Return intAnnouncedNum
    End Function

    Private Sub chkBoardsForNum(ByVal intColumnNum As Integer)
        'blnUserHasAnnouncedNum = False

        For index1 As Integer = 1 To intNumOfPlayers                ' For each player.
            For index2 As Integer = 0 To (intNumOfBoards - 1)       ' For each player's board.
                ' Column was given as a parameter.
                For index4 As Integer = 0 To 4                      ' For each player's board's row.
                    If (intSquaresNums(index1, index2, index4, intColumnNum) = intAnnouncedNum) Then
                        blnMarkedSquares(index1, index2, index4, intColumnNum) = True
                    End If
                Next
            Next
        Next
    End Sub

    Public Function chkUserForAnnoucedNum(ByVal intColumnNum As Integer) As Boolean
        For index1 As Integer = 0 To (intNumOfBoards - 1)
            For index2 As Integer = 0 To 4
                If (intSquaresNums(0, index1, index2, intColumnNum) = intAnnouncedNum) Then
                    If Not blnMarkedSquares(0, index1, index2, intColumnNum) Then
                        Return True
                    End If
                End If
            Next
        Next
        Return False
    End Function

    Public ReadOnly Property ColumnNum()
        Get
            Return intColumnNum
        End Get
    End Property

    Public ReadOnly Property ColumnLetter()
        Get
            Select Case intColumnNum
                Case 0
                    Return "B"
                Case 1
                    Return "I"
                Case 2
                    Return "N"
                Case 3
                    Return "G"
                Case 4
                    Return "O"
            End Select
#Disable Warning BC42107 ' Property doesn't return a value on all code paths
        End Get
#Enable Warning BC42107 ' Property doesn't return a value on all code paths
    End Property

    Public Function isBallLeft() As Boolean
        Return (intCalledNums.Count < 75)
    End Function

    Public Sub setUserMarks(ByRef blnUserMarks(,) As Boolean, ByVal intBoardNum As Integer)
        For index1 As Integer = 0 To 4
            For index2 As Integer = 0 To 4
                blnMarkedSquares(0, (intBoardNum - 1), index1, index2) = blnUserMarks(index1, index2)
            Next
        Next
    End Sub

    Public Sub determineWinner()
        Dim intPlayerNum As Integer
        Dim intBoardNum As Integer

        For intPlayerNum = 0 To intNumOfPlayers                ' For each player.
            For intBoardNum = 0 To (intNumOfBoards - 1)       ' For each player's board.
                If isVerticalVictory(intPlayerNum, intBoardNum) Then
                    announceWinner(intPlayerNum, intBoardNum, 0)
                ElseIf isHorizontalVictory(intPlayerNum, intBoardNum) Then
                    announceWinner(intPlayerNum, intBoardNum, 1)
                ElseIf isDiagonalVictory(intPlayerNum, intBoardNum) Then
                    announceWinner(intPlayerNum, intBoardNum, 1)
                End If
            Next
        Next
    End Sub

    Private Function isVerticalVictory(ByVal intPlayerNum As Integer, ByVal intBoardNum As Integer) As Boolean
        Dim blnIsVistory As Boolean = False
        For intColumnNum As Integer = 0 To 4
            For intRowNum As Integer = 0 To 4
                If blnMarkedSquares(intPlayerNum, intBoardNum, intRowNum, intColumnNum) Then
                    blnIsVistory = True
                    If intRowNum = 4 Then
                        Return blnIsVistory
                    End If
                Else
                    blnIsVistory = False
                    Exit For
                End If
            Next
        Next
        Return blnIsVistory
    End Function

    Private Function isHorizontalVictory(ByVal intPlayerNum As Integer, ByVal intBoardNum As Integer) As Boolean
        Dim blnIsVistory As Boolean = False
        For intRowNum As Integer = 0 To 4
            For intColumnNum As Integer = 0 To 4
                If blnMarkedSquares(intPlayerNum, intBoardNum, intRowNum, intColumnNum) Then
                    blnIsVistory = True
                    If intColumnNum = 4 Then
                        Return blnIsVistory
                    End If
                Else
                    blnIsVistory = False
                    Exit For
                End If
            Next
        Next
        Return blnIsVistory
    End Function

    Private Function isDiagonalVictory(ByVal intPlayerNum As Integer, ByVal intBoardNum As Integer) As Boolean
        If blnMarkedSquares(intPlayerNum, intBoardNum, 0, 0) Then
            If blnMarkedSquares(intPlayerNum, intBoardNum, 1, 1) Then
                If blnMarkedSquares(intPlayerNum, intBoardNum, 2, 2) Then
                    If blnMarkedSquares(intPlayerNum, intBoardNum, 3, 3) Then
                        If blnMarkedSquares(intPlayerNum, intBoardNum, 4, 4) Then
                            Return True
                        End If
                    End If
                End If
            End If
        End If
        If blnMarkedSquares(intPlayerNum, intBoardNum, 0, 4) Then
            If blnMarkedSquares(intPlayerNum, intBoardNum, 1, 3) Then
                If blnMarkedSquares(intPlayerNum, intBoardNum, 2, 2) Then
                    If blnMarkedSquares(intPlayerNum, intBoardNum, 3, 1) Then
                        If blnMarkedSquares(intPlayerNum, intBoardNum, 4, 0) Then
                            Return True
                        End If
                    End If
                End If
            End If
        End If
        Return False
    End Function

    Private Sub announceWinner(ByVal intPlayerNum As Integer, ByVal intBoardNum As Integer, ByVal intLineType As Integer)
        Dim userAnswer As DialogResult
        Dim strLineType As String = String.Empty

        Select Case intLineType
            Case 0
                strLineType = "vertical"
            Case 1
                strLineType = "horizontal"
            Case 2
                strLineType = "diagonal"
        End Select
        Select Case intPlayerNum
            Case 0
                MessageBox.Show("You have made a " & strLineType & " victory on board #" & (intBoardNum + 1) & "." &
                                vbNewLine & vbNewLine & "CHECK IT OUT!!!")
            Case 1
                MessageBox.Show("Player #2 has made a " & strLineType & " victory on board #" & (intBoardNum + 1) & "." &
                                vbNewLine & vbNewLine & "CHECK IT OUT!!!")
            Case 2
                MessageBox.Show("Player #3 has made a " & strLineType & " victory on board #" & (intBoardNum + 1) & "." &
                                vbNewLine & vbNewLine & "CHECK IT OUT!!!")
            Case 3
                MessageBox.Show("Player #4 has made a " & strLineType & " victory on board #" & (intBoardNum + 1) & "." &
                                vbNewLine & vbNewLine & "CHECK IT OUT!!!")
        End Select
        blnIsGameOver = True
    End Sub

    Friend ReadOnly Property IsGameOver()
        Get
            Return blnIsGameOver
        End Get
    End Property

End Class