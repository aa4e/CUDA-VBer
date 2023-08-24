Namespace CudaVber

    Public Interface ICudaResult(Of T)

        ReadOnly Property [Error] As CudaError
        ReadOnly Property Result As T

    End Interface

End Namespace