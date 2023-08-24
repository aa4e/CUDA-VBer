Namespace CudaVber

    Public Class CudaResult(Of T)
        Implements ICudaResult(Of T)

        Public ReadOnly Property [Error] As CudaError Implements ICudaResult(Of T).Error
        Public ReadOnly Property Result As T Implements ICudaResult(Of T).Result

        Public Sub New(err As CudaError, result As T)
            Me.Error = err
            Me.Result = result
        End Sub

        Public Sub New(err As Integer, result As T)
            Me.Error = DTM.CudaErrorCodes(err)
            Me.Result = result
        End Sub

    End Class

End Namespace