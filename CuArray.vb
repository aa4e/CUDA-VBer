Namespace CudaVber

    ''' <summary>
    ''' Manipulating arrays: splitting and merging arrays, or adding and subtracting arrays.
    ''' </summary>
    Public NotInheritable Class CuArray
        Implements IDisposable

#Region "CTORs"

        Shared Sub New()
            CudaSettings.Load()
        End Sub

        Public Sub New(device As ICudaDevice)
            CudaDeviceComponent = New CudaDevice(device.DeviceId, device.AllocationSize)
            PtrToUnmanagedClass = SafeNativeMethods.CreateArrayClass(CudaDeviceComponent.DeviceId, CudaDeviceComponent.AllocationSize)
        End Sub

#End Region '/CTORs

#Region "PROPS"

        Private ReadOnly Property CudaDeviceComponent As ICudaDevice
        Private Property PtrToUnmanagedClass As IntPtr

        Public ReadOnly Property DeviceId As Integer
            Get
                Return CudaDeviceComponent.DeviceId
            End Get
        End Property

#End Region '/PROPS

#Region "METHODS"

        Public Function Add(array1 As Integer(), array2 As Integer()) As ICudaResult(Of Integer())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Integer
            Dim err = SafeNativeMethods.AddIntArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Integer())(err, result)
        End Function

        Public Function Add(array1 As Single(), array2 As Single()) As ICudaResult(Of Single())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Single
            Dim err = SafeNativeMethods.AddFloatArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Single())(err, result)
        End Function

        Public Function Add(array1 As Long(), array2 As Long()) As ICudaResult(Of Long())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Long
            Dim err = SafeNativeMethods.AddLongArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Long())(err, result)
        End Function

        Public Function Add(array1 As Double(), array2 As Double()) As ICudaResult(Of Double())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Double
            Dim err = SafeNativeMethods.AddDoubleArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Double())(err, result)
        End Function

        Public Function Subtract(array1 As Integer(), array2 As Integer()) As ICudaResult(Of Integer())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Integer
            Dim err = SafeNativeMethods.SubtractIntArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Integer())(err, result)
        End Function

        Public Function Subtract(array1 As Single(), array2 As Single()) As ICudaResult(Of Single())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Single
            Dim err = SafeNativeMethods.SubtractFloatArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Single())(err, result)
        End Function

        Public Function Subtract(array1 As Long(), array2 As Long()) As ICudaResult(Of Long())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Long
            Dim err = SafeNativeMethods.SubtractLongArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Long())(err, result)
        End Function

        Public Function Subtract(array1 As Double(), array2 As Double()) As ICudaResult(Of Double())
            If (array1.Length <> array2.Length) Then
                Throw New ArgumentOutOfRangeException("Bad arrays given they need to be the same length.")
            End If
            Dim result(array1.Length - 1) As Double
            Dim err = SafeNativeMethods.SubtractDoubleArrays(PtrToUnmanagedClass, result, array1, array2, array1.Length)
            Return New CudaResult(Of Double())(err, result)
        End Function

        Private Function MatrixSizeByOperation(rows As Integer, columns As Integer, operation As CUBLAS_OP) As ColsRows ' (Rows As Integer, Columns As Integer)
            Dim matrixRows As Integer = 0
            Dim matricColumns As Integer = 0
            Select Case operation
                Case CUBLAS_OP.DO_NOT_TRANSPOSE
                    matrixRows = rows
                    matricColumns = columns
                Case CUBLAS_OP.TRANSPOSE
                    matrixRows = columns
                    matricColumns = rows
            End Select
            Return New ColsRows(matrixRows, matricColumns)
        End Function

        Public Function Multiply(a_op As CUBLAS_OP, b_op As CUBLAS_OP, alpha As Single, a As Single()(), b As Single()(), beta As Single) As ICudaResult(Of Single()())
            ' C(m, n) = A(m, k) * B(k, n)
            Dim matrix_a_dimensions = MatrixSizeByOperation(a.Length, a(0).Length, a_op)
            Dim matrix_b_dimensions = MatrixSizeByOperation(b.Length, b(0).Length, b_op)

            If (matrix_a_dimensions.Columns <> matrix_b_dimensions.Rows) Then
                Throw New ArgumentOutOfRangeException($"Matrices provided cannot be multipled. Columns in matrix A: {matrix_a_dimensions.Columns} vs rows in matrix B: {matrix_b_dimensions.Rows}")
            End If

            Dim result = DTM.MatrixMultiplyFloat(DeviceId, a_op, b_op, alpha, a, b, beta)
            Return New CudaResult(Of Single()())(result.Error, result.Result)
        End Function

        Public Function Multiply(a_op As CUBLAS_OP, b_op As CUBLAS_OP, alpha As Double, a As Double()(), b As Double()(), beta As Double) As ICudaResult(Of Double()())
            ' C(m, n) = A(m, k) * B(k, n)
            Dim matrix_a_dimensions = MatrixSizeByOperation(a.Length, a(0).Length, a_op)
            Dim matrix_b_dimensions = MatrixSizeByOperation(b.Length, b(0).Length, b_op)

            If (matrix_a_dimensions.Columns <> matrix_b_dimensions.Rows) Then
                Throw New ArgumentOutOfRangeException($"Matrices provided cannot be multipled. Columns in matrix A: {matrix_a_dimensions.Columns} vs rows in matrix B: {matrix_b_dimensions.Rows}")
            End If

            Dim result = DTM.MatrixMultiplyDouble(DeviceId, a_op, b_op, alpha, a, b, beta)
            Return New CudaResult(Of Double()())(result.Error, result.Result)
        End Function

#End Region '/METHODS

#Region "IDISPOSABLE"

        Private DisposedValue As Boolean

        Protected Sub Dispose(disposing As Boolean)
            If (Not DisposedValue) Then
                If disposing Then
                    'dispose managed state (managed objects)
                End If

                'free unmanaged resources (unmanaged objects) and override finalizer, set large fields to null
                If (PtrToUnmanagedClass <> IntPtr.Zero) Then
                    SafeNativeMethods.DisposeArrayClass(PtrToUnmanagedClass)
                    PtrToUnmanagedClass = IntPtr.Zero
                End If

                DisposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region '/IDISPOSABLE

    End Class

End Namespace