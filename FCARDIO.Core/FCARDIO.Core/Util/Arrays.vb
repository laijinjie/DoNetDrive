Namespace Util
    Public Class Arrays(Of T)
        ''' <summary>
        ''' 拷贝数组
        ''' </summary>
        ''' <param name="src">源数组，从此数组中取数据</param>
        ''' <param name="iBegin">开始截取数组的起始索引，从0开始。</param>
        ''' <param name="lLen">截取数组元素的长度。</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function copyOfRange(ByVal src() As T, ByVal iBegin As Integer, ByVal lLen As Integer) As T()
            Dim bTmp() As T
            ReDim bTmp(lLen - 1)
            Array.Copy(src, iBegin, bTmp, 0, lLen)
            Return bTmp
        End Function


        ''' <summary>
        ''' 检查两个数组是否匹配
        ''' </summary>
        ''' <param name="src"></param>
        ''' <param name="sDec"></param>
        ''' <returns>true--匹配；false--不匹配</returns>
        ''' <remarks></remarks>
        Public Shared Function BytesEquals(ByVal src() As T, ByVal sDec() As T) As Boolean
            'Dim bret As List(Of T) = New List(Of T)
            Dim iCount As Integer = 0
            If src Is Nothing Then
                If sDec Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If

            If sDec Is Nothing Then
                If src Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End If
            If sDec.Length <> src.Length Then
                Return False
            End If

            Dim i As Integer
            For i = 0 To src.Length - 1
                If Not src(i).Equals(sDec(i)) Then
                    iCount += 1
                    Exit For
                End If
            Next

            If iCount = 0 Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class
End Namespace


