Namespace CudaVber

    ''' <summary>
    ''' Statistical functions for large data sets: sample standard deviation etc.
    ''' </summary>
    Public Class CuStats
        Implements IDisposable

#Region "PROPS"

        Private ReadOnly Property CudaDeviceComponent As ICudaDevice
        Private Property PtrToUnmanagedClass As IntPtr
        Public ReadOnly Property DeviceId As Integer = CudaDeviceComponent.DeviceId

#End Region '/PROPS

#Region "CTORs"

        Shared Sub New()
            CudaSettings.Load()
        End Sub

        Public Sub New(device As ICudaDevice)
            CudaDeviceComponent = New CudaDevice(device.DeviceId, device.AllocationSize)
            PtrToUnmanagedClass = SafeNativeMethods.CreateStatClass(CudaDeviceComponent.DeviceId, CudaDeviceComponent.AllocationSize)
        End Sub

#End Region '/CTORs

#Region "METHODS"

        Public Function SampleStandardDeviation(sample As Single(), mean As Single) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.SampleStandardDeviationFloat(PtrToUnmanagedClass, result, sample, sample.LongLength, mean)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function SampleStandardDeviation(sample As Single()) As ICudaResult(Of Double)
            Return SampleStandardDeviation(sample, sample.Average())
        End Function

        Public Function SampleStandardDeviation(sample As Double(), mean As Double) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.SampleStandardDeviationDouble(PtrToUnmanagedClass, result, sample, sample.LongLength, mean)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function SampleStandardDeviation(sample As Double()) As ICudaResult(Of Double)
            Return SampleStandardDeviation(sample, sample.Average())
        End Function

        Public Function StandardDeviation(sample As Single(), mean As Single) As ICudaResult(Of Double)
            If (sample.LongLength > CudaDeviceComponent.AllocationSize) Then
                Throw New ArgumentOutOfRangeException("Array bigger than allocation size.")
            End If

            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.StandardDeviationFloat(PtrToUnmanagedClass, result, sample, sample.LongLength, mean)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function StandardDeviation(sample As Single()) As ICudaResult(Of Double)
            Return StandardDeviation(sample, sample.Average())
        End Function

        Public Function StandardDeviation(sample As Double(), mean As Double) As ICudaResult(Of Double)
            If (sample.LongLength > CudaDeviceComponent.AllocationSize) Then
                Throw New ArgumentOutOfRangeException("Array bigger than allocation size.")
            End If

            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.StandardDeviationDouble(PtrToUnmanagedClass, result, sample, sample.LongLength, mean)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function StandardDeviation(sample As Double()) As ICudaResult(Of Double)
            Return StandardDeviation(sample, sample.Average())
        End Function

        Public Function Variance(array As Single(), mean As Single) As ICudaResult(Of Double)
            Dim std = StandardDeviation(array, mean)
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function Variance(array As Single()) As ICudaResult(Of Double)
            Dim std = StandardDeviation(array, array.Average())
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function Variance(array As Double(), mean As Double) As ICudaResult(Of Double)
            Dim std = StandardDeviation(array, mean)
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function Variance(array As Double()) As ICudaResult(Of Double)
            Dim std = StandardDeviation(array, array.Average())
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function SampleVariance(array As Single(), mean As Single) As ICudaResult(Of Double)
            Dim std = SampleStandardDeviation(array, mean)
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function SampleVariance(array As Single()) As ICudaResult(Of Double)
            Dim std = SampleStandardDeviation(array, array.Average())
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function SampleVariance(array As Double(), mean As Double) As ICudaResult(Of Double)
            Dim std = SampleStandardDeviation(array, mean)
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function SampleVariance(array As Double()) As ICudaResult(Of Double)
            Dim std = SampleStandardDeviation(array, array.Average())
            Return New CudaResult(Of Double)(std.Error, Math.Pow(std.Result, 2))
        End Function

        Public Function SampleCovariance(x_array As Single(), x_mean As Single, y_array As Single(), y_mean As Single) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.SampleCovarianceFloat(PtrToUnmanagedClass, result, x_array, x_mean, y_array, y_mean, x_array.LongLength)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function SampleCovariance(x_array As Single(), y_array As Single()) As ICudaResult(Of Double)
            Return SampleCovariance(x_array, x_array.Average(), y_array, y_array.Average())
        End Function

        Public Function SampleCovariance(x_array As Double(), x_mean As Double, y_array As Double(), y_mean As Double) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.SampleCovarianceDouble(PtrToUnmanagedClass, result, x_array, x_mean, y_array, y_mean, x_array.LongLength)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function SampleCovariance(x_array As Double(), y_array As Double()) As ICudaResult(Of Double)
            Return SampleCovariance(x_array, x_array.Average(), y_array, y_array.Average())
        End Function

        Public Function Covariance(x_array As Double(), x_mean As Double, y_array As Double(), y_mean As Double) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.CovarianceDouble(PtrToUnmanagedClass, result, x_array, x_mean, y_array, y_mean, x_array.LongLength)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function Covariance(x_array As Single(), x_mean As Single, y_array As Single(), y_mean As Single) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.CovarianceFloat(PtrToUnmanagedClass, result, x_array, x_mean, y_array, y_mean, x_array.LongLength)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function Covariance(x_array As Double(), y_array As Double()) As ICudaResult(Of Double)
            Return Covariance(x_array, x_array.Average(), y_array, y_array.Average())
        End Function

        Public Function Covariance(x_array As Single(), y_array As Single()) As ICudaResult(Of Double)
            Return Covariance(x_array, x_array.Average(), y_array, y_array.Average())
        End Function

        Public Function Correlation(x_array As Single(), x_mean As Single, y_array As Single(), y_mean As Single) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.PearsonCorrelationFloat(PtrToUnmanagedClass, result, x_array, x_mean, y_array, y_mean, x_array.LongLength)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function Correlation(x_array As Single(), y_array As Single()) As ICudaResult(Of Double)
            Return Correlation(x_array, x_array.Average(), y_array, y_array.Average())
        End Function

        Public Function Correlation(x_array As Double(), x_mean As Double, y_array As Double(), y_mean As Double) As ICudaResult(Of Double)
            Dim result As Double = 0
            Dim errorCode = SafeNativeMethods.PearsonCorrelationDouble(PtrToUnmanagedClass, result, x_array, x_mean, y_array, y_mean, x_array.LongLength)
            Return New CudaResult(Of Double)(errorCode, result)
        End Function

        Public Function Correlation(x_array As Double(), y_array As Double()) As ICudaResult(Of Double)
            Return Correlation(x_array, x_array.Average(), y_array, y_array.Average())
        End Function

        Public Function Autocorrelation(x_array As Single(), x_mean As Single, y_array As Single(), y_mean As Single, lag As Integer) As ICudaResult(Of Double)
            If (lag < 0) Then
                Throw New ArgumentOutOfRangeException($"Lag cannot be less than 0. Given: {lag}")
            End If

            Dim length As Integer = CInt(x_array.LongLength - lag) 'TEST Overflow?
            Dim array1(length - 1) As Double
            Dim array2(length - 1) As Double
            Array.Copy(x_array, 0, array1, 0, length)
            Array.Copy(y_array, lag, array2, 0, length)

            If (array1.Length <> array2.Length) Then
                Throw New AccessViolationException($"Array size mismatch: {array1.Length} vs {array2.Length}")
            End If
            If (array1.Length > CudaDeviceComponent.AllocationSize) Then
                Throw New ArgumentOutOfRangeException($"Array bigger than allocation size: {array1.Length} vs {CudaDeviceComponent.AllocationSize}")
            End If

            Dim std = Variance(x_array, x_mean)
            Dim errorCode = CudaError.Success
            If (std.Error <> CudaError.Success) Then
                errorCode = std.Error
            End If

            Dim cov = Covariance(array1, x_mean, array2, y_mean)
            If (cov.Error <> CudaError.Success) Then
                errorCode = cov.Error
            End If

            Return New CudaResult(Of Double)(errorCode, cov.Result / std.Result)
        End Function

        Public Function Autocorrelation(x_array As Single(), y_array As Single(), lag As Integer) As ICudaResult(Of Double)
            Return Autocorrelation(x_array, x_array.Average(), y_array, y_array.Average(), lag)
        End Function

        Public Function Autocorrelation(x_array As Double(), x_mean As Double, y_array As Double(), y_mean As Double, lag As Integer) As ICudaResult(Of Double)
            If (lag < 0) Then
                Throw New ArgumentOutOfRangeException($"Lag cannot be less than 0. Given: {lag}")
            End If


            Dim length As Integer = CInt(x_array.LongLength - lag) 'TEST Overflow?
            Dim array1(length - 1) As Double
            Dim array2(length - 1) As Double
            Array.Copy(x_array, 0, array1, 0, length)
            Array.Copy(y_array, lag, array2, 0, length)

            If (array1.Length <> array2.Length) Then
                Throw New AccessViolationException($"Array size mismatch: {array1.Length} vs {array2.Length}")
            End If
            If (array1.Length > CudaDeviceComponent.AllocationSize) Then
                Throw New ArgumentOutOfRangeException($"Array bigger than allocation size: {array1.Length} vs {CudaDeviceComponent.AllocationSize}")
            End If

            Dim std = Variance(x_array, x_mean)
            Dim errorCode = CudaError.Success
            If (std.Error <> CudaError.Success) Then
                errorCode = std.Error
            End If

            Dim cov = Covariance(array1, x_mean, array2, y_mean)
            If (cov.Error <> CudaError.Success) Then
                errorCode = cov.Error
            End If

            Return New CudaResult(Of Double)(errorCode, cov.Result / std.Result)
        End Function

        Public Function Autocorrelation(x_array As Double(), y_array As Double(), lag As Integer) As ICudaResult(Of Double)
            Return Autocorrelation(x_array, x_array.Average(), y_array, y_array.Average(), lag)
        End Function

        Public Function CorrelationMatrix(setsOfScalars As Single()()) As ICudaResult(Of Double()())
            Dim setLength As Integer = CInt(setsOfScalars.LongLength) 'TEST Overflow?
            Dim c(setLength - 1)() As Double
            Dim errorCode = CudaError.Success

            For i As Integer = 0 To setLength - 1
                ReDim c(i)(setLength - 1)

                For j As Integer = 0 To setLength - 1
                    ' Correlation(x, y) will always return a double, but it will use FP32 if given floats or FP64 given doubles.
                    Dim corr = Correlation(setsOfScalars(i), setsOfScalars(j))
                    errorCode = If(corr.Error <> CudaError.Success, corr.Error, CudaError.Success)
                    c(i)(j) = corr.Result
                Next
            Next

            Return New CudaResult(Of Double()())(errorCode, c)
        End Function

        Public Function CorrelationMatrix(setsOfScalars As Double()()) As ICudaResult(Of Double()())
            Dim setLength As Integer = CInt(setsOfScalars.LongLength) 'TEST Overflow?
            Dim c(setLength - 1)() As Double
            Dim errorCode = CudaError.Success

            For i As Integer = 0 To setLength - 1
                ReDim c(i)(setLength - 1)

                For j As Integer = 0 To setLength - 1
                    Dim corr = Correlation(setsOfScalars(i), setsOfScalars(j))
                    errorCode = If(corr.Error <> CudaError.Success, corr.Error, CudaError.Success)
                    c(i)(j) = corr.Result
                Next
            Next

            Return New CudaResult(Of Double()())(errorCode, c)
        End Function

        Public Function CovarianceMatrix(setsOfScalars As Single()()) As ICudaResult(Of Double()())
            Dim setLength As Integer = CInt(setsOfScalars.LongLength) 'TEST Overflow?
            Dim c(setLength - 1)() As Double
            Dim errorCode = CudaError.Success

            For i As Integer = 0 To setLength - 1
                ReDim c(i)(setLength - 1)

                For j As Integer = 0 To setLength - 1
                    Dim cov = Covariance(setsOfScalars(i), setsOfScalars(j))
                    errorCode = If(cov.Error <> CudaError.Success, cov.Error, CudaError.Success)
                    c(i)(j) = cov.Result
                Next
            Next

            Return New CudaResult(Of Double()())(errorCode, c)
        End Function

        Public Function CovarianceMatrix(setsOfScalars As Double()()) As ICudaResult(Of Double()())
            Dim setLength As Integer = CInt(setsOfScalars.LongLength) 'TEST Overflow?
            Dim c(setLength - 1)() As Double
            Dim errorCode = CudaError.Success

            For i As Integer = 0 To setLength - 1
                ReDim c(i)(setLength - 1)

                For j As Integer = 0 To setLength - 1
                    Dim cov = Covariance(setsOfScalars(i), setsOfScalars(j))
                    errorCode = If(cov.Error <> CudaError.Success, cov.Error, CudaError.Success)
                    c(i)(j) = cov.Result
                Next
            Next

            Return New CudaResult(Of Double()())(errorCode, c)
        End Function

        ''' <summary>
        ''' Calculates the Value-at-Risk for a portfolio.
        ''' </summary>
        ''' <param name="investedAmounts">An array (1xN matrix) of the amounts invested in each portfolio.</param>
        ''' <param name="covarianceMatrix">A covariance matrix (NxN matrix) of the portfolio for the given time period.</param>
        ''' <param name="confidenceLevel">The confidence level. This should be in units of standard deviation of a normal distribution (e.g., 0.90 = 1.645).</param>
        ''' <param name="timePeriod">The time period for measuring risk.</param>
        ''' <returns>The Value-at-Risk. No units involved.</returns>
        Public Function VaR(investedAmounts As Single(), covarianceMatrix As Single()(), confidenceLevel As Double, timePeriod As Integer) As Double
            Using cuArray As New CuArray(New CudaDevice(CudaDeviceComponent.DeviceId, CudaDeviceComponent.AllocationSize))

                Dim invested_amounts_horizontal = New Single()() {investedAmounts} 'TEST 
                Dim covariance_times_beta_horizontal = cuArray.Multiply(
                    CUBLAS_OP.DO_NOT_TRANSPOSE, CUBLAS_OP.DO_NOT_TRANSPOSE,
                    1,
                    invested_amounts_horizontal,
                    covarianceMatrix,
                    0)

                Dim covariance_times_beta_vertical = cuArray.Multiply(
                    CUBLAS_OP.TRANSPOSE, CUBLAS_OP.DO_NOT_TRANSPOSE,
                    1,
                    covariance_times_beta_horizontal.Result,
                    invested_amounts_horizontal,
                    0)

                If (covariance_times_beta_vertical.Result.Length > 1 OrElse covariance_times_beta_vertical.Result(0).Length > 1) Then
                    Throw New ArgumentOutOfRangeException("The matrix given for Beta * CovMatrix * Beta^T was bigger than one.")
                End If

                Return Math.Sqrt(covariance_times_beta_vertical.Result(0)(0)) * confidenceLevel * Math.Sqrt(timePeriod)
            End Using
        End Function

#End Region '/METHODS

#Region "IDISPOSABLE"

        Private DisposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If (Not DisposedValue) Then
                If disposing Then
                    ' dispose managed state (managed objects)
                End If

                'free unmanaged resources (unmanaged objects) and override finalizer, set large fields to null
                If (PtrToUnmanagedClass <> IntPtr.Zero) Then
                    SafeNativeMethods.DisposeStatClass(PtrToUnmanagedClass)
                    PtrToUnmanagedClass = IntPtr.Zero
                End If

                DisposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

#End Region '/IDISPOSABLE

    End Class

End Namespace