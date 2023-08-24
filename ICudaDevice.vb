Namespace CudaVber

    Public Interface ICudaDevice

        ReadOnly Property DeviceName As String  'This property Is for WPF.

        Function GetCudaDeviceName() As String

        ReadOnly Property DeviceId As Integer

        ''' <summary>
        ''' The amount of memory that can be allocated.
        ''' </summary>
        ''' <remarks>
        ''' CSL will only allocate memory as it is requested. 
        ''' Further, any calls to this device cannot reference more than this amount, but they can reference less than it.
        ''' </remarks>
        ReadOnly Property AllocationSize As Long

    End Interface

End Namespace