
'The starting point of CudaSharper is the CUDA C library called CudaSharperLibrary (CSL).
'These functions, defined in C, are P/Invoked to the CudaSharper wrapper (CSW).
'The wrapper is segmented into three responsibilities: the marshaler, the translator, and the exposer.
'
'The marshaler: defined in SafeNativeMethods. These are static extern methods.
'
'The translator: DataTranslationMethods(DTM), which is tasked with translating data types and structures between CSL and CSW.
'This is primarily used for cuBLAS, which uses a column-major in constrast to the row-major notation in C/C++/C#/etc.
'
'Side note: ALL CUDA functions must return ICudaResult(Of T). 
'This can be used to convert the int returned by SafeNativeMethods into CudaError. 
'This eliminates the need to duplicate code from SafeNativeMethods to DTM.
'
'The exposer: This is the API that external libraries will call. 
'These methods and classes should be entirely written in C# and should not any idea they are using marshaled data and functions.
'Any sort of argument validation should be done here as well.
'
'As a diagram, it looks Like this:
'
'C# App -> Exposer -> DTM -> Marshaler -> CUDA C -> Marshaler -> DTM -> Exposer -> C# App

Namespace CudaVber

    ''' <summary>
    ''' DTM = Data Translation Methods.
    ''' </summary>
    Friend Module DTM

        Friend Function CudaErrorCodes(errorCode As Integer) As CudaError
            If ([Enum].IsDefined(GetType(CudaError), errorCode)) Then
                Return CType(errorCode, CudaError)
            Else
                Throw New ArgumentOutOfRangeException($"Provided CUDA Error code {errorCode} is unknown.")
            End If
        End Function

        Friend Function FlattenArray(Of T)(rows As Integer, columns As Integer, nestedArray As T()()) As T()
            Dim flatArray(rows * columns - 1) As T
            For y As Integer = 0 To rows - 1
                For x As Integer = 0 To columns - 1
                    flatArray((y * rows) + x) = nestedArray(y)(x)
                Next
            Next
            Return flatArray
        End Function

        Friend Function UnflattenArray(Of T)(rows As Integer, columns As Integer, flatArray As T()) As T()()
            Dim nestedArray(rows - 1)() As T
            For y As Integer = 0 To rows - 1

                ReDim nestedArray(y)(columns - 1) 'TEST  nestedArray(y) = New T(columns - 1) 

                For x As Integer = 0 To columns - 1
                    nestedArray(y)(x) = flatArray((y * rows) + x)
                Next
            Next

            Return nestedArray
        End Function

#Region "CudaDevice"

        Friend Function GetCudaDeviceName(deviceId As Integer) As CudaResultTuple(Of String)
            Dim deviceName As New Text.StringBuilder(256)
            Dim err = SafeNativeMethods.GetCudaDeviceName(deviceId, deviceName)
            Return New CudaResultTuple(Of String)(CudaErrorCodes(err), deviceName.ToString())
        End Function

#End Region

#Region "CudaSettings"

        Friend Function GetCudaDeviceCount() As CudaError
            Return CudaErrorCodes(SafeNativeMethods.GetCudaDeviceCount())
        End Function

        Friend Function ResetCudaDevice() As CudaError
            Return CudaErrorCodes(SafeNativeMethods.ResetCudaDevice())
        End Function

#End Region

        Friend Function MatrixMultiplyFloat(deviceId As Integer, a_op As CUBLAS_OP, b_op As CUBLAS_OP, alpha As Single, a As Single()(), b As Single()(), beta As Single) As CudaResultTuple(Of Single()())

            ' .NET does not support marshaling nested arrays between C++ and e.g. C#.
            ' If you try, you will get the error message, "There is no marshaling support for nested arrays."
            ' Further, the cuBLAS function cublasSgemm/cublasDgemm does not have pointer-to-pointers as arguments (e.g., Single**), so we cannot
            ' supply a multidimensional array anyway.
            ' The solution: flatten arrays so that they can passed to CudaSharperLibrary, and then unflatten whatever it passes back.
            Dim d_a = FlattenArray(a.Length, a(0).Length, a)
            Dim d_b = FlattenArray(b.Length, b(0).Length, b)

            ' C(m,n) = A(m,k) * B(k,n)
            ' Despite the definition above, this will return the correct size for C. Go figure.
            Dim d_c(a.Length * b.Length - 1) As Single

            Dim transa_op = CInt(a_op)
            Dim transb_op = CInt(b_op)

            Dim err = SafeNativeMethods.MatrixMultiplyFloat(deviceId, transa_op, transb_op, a.Length, b(0).Length, a(0).Length, alpha, d_a, d_b, beta, d_c)

            Return New CudaResultTuple(Of Single()())(CudaErrorCodes(err), UnflattenArray(a.Length, b.Length, d_c))
        End Function

        Friend Function MatrixMultiplyDouble(deviceId As Integer, a_op As CUBLAS_OP, b_op As CUBLAS_OP, alpha As Double, a As Double()(), b As Double()(), beta As Double) As CudaResultTuple(Of Double()())

            ' .NET does Not support marshaling nested arrays between C++ And e.g. C#.
            ' If you try, you will get the error message, "There is no marshaling support for nested arrays."
            ' Further, the cuBLAS function cublasSgemm/cublasDgemm does Not have pointer-to-pointers as arguments (e.g., Single**), so we cannot
            ' supply a multidimensional array anyway.
            ' The solution: flatten arrays so that they can passed To CudaSharperLibrary, And Then unflatten whatever it passes back.
            Dim d_a = FlattenArray(a.Length, a(0).Length, a)
            Dim d_b = FlattenArray(b.Length, b(0).Length, b)

            ' C(m,n) = A(m,k) * B(k,n)
            ' Despite the definition above, this will return the correct size for C. Go figure.
            Dim d_c(a.Length * b.Length - 1) As Double

            Dim transa_op = CInt(a_op)
            Dim transb_op = CInt(b_op)

            Dim err = SafeNativeMethods.MatrixMultiplyDouble(deviceId, transa_op, transb_op, a.Length, b(0).Length, a(0).Length, alpha, d_a, d_b, beta, d_c)

            Return New CudaResultTuple(Of Double()())(CudaErrorCodes(err), UnflattenArray(a.Length, b.Length, d_c))
        End Function

    End Module

End Namespace