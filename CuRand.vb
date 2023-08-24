Namespace CudaVber

    ''' <summary>
    ''' cuRAND functions allows the generation of pseudorandom number sets using uniform, standard normal or Poisson distributions.
    ''' </summary>
    Public Class CuRand
        Implements IDisposable

#Region "PROPS"

        Private ReadOnly Property CudaDeviceComponent As ICudaDevice
        Private Property PtrToUnmanagedClass As IntPtr

        Public ReadOnly Property DeviceId As Integer
            Get
                Return CudaDeviceComponent.DeviceId
            End Get
        End Property

        Public ReadOnly Property Max As Long
            Get
                Return CudaDeviceComponent.AllocationSize
            End Get
        End Property


#End Region '/PROPS

#Region "CTORs"

        Shared Sub New()
            CudaSettings.Load()
        End Sub

        Public Sub New(device As ICudaDevice)
            CudaDeviceComponent = New CudaDevice(device.DeviceId, device.AllocationSize)
            PtrToUnmanagedClass = SafeNativeMethods.CreateRandomClass(CudaDeviceComponent.DeviceId, CudaDeviceComponent.AllocationSize)
        End Sub

#End Region '/CTORs

#Region "METHODS"

        Public Function GenerateUniformDistribution(amountOfNumbers As Long, result As Single()) As ICudaResult(Of Single())
            Dim err = SafeNativeMethods.UniformRand(PtrToUnmanagedClass, result, amountOfNumbers)
            Return New CudaResult(Of Single())(err, result)
        End Function

        ''' <summary>
        ''' Generates random numbers using XORWOW and a uniform distribution.
        ''' This method utilizies the single-precision (FP32) capabilities of the GPU.
        ''' If you need higher precision, there is a double-precision (FP64) version available however, performance will be much worse, depending on the GPU being used.
        ''' </summary>
        ''' <param name="amountOfNumbers">The amount of random numbers to generate.</param>
        ''' <returns>An IEnumerable holding the random numbers (in memory for the CPU to use).</returns>
        Public Function GenerateUniformDistribution(amountOfNumbers As Long) As ICudaResult(Of Single())
            Dim result(CInt(amountOfNumbers - 1)) As Single 'TEST Overflow?
            Return GenerateUniformDistribution(amountOfNumbers, result)
        End Function

        Public Function GenerateUniformDistributionDP(amountOfNumbers As Long, result As Double()) As ICudaResult(Of Double())
            Dim err = SafeNativeMethods.UniformRandDouble(PtrToUnmanagedClass, result, amountOfNumbers)
            Return New CudaResult(Of Double())(err, result)
        End Function

        ''' <summary>
        ''' Generate random numbers using XORWOW and a uniform distribution. This method utilizes the double-precision (FP64) capabilities of the GPU this will perform worse than 
        ''' using the single-precision (FP32) capabilities, and much worse on GeForce versus Quadro and Tesla. (Recommend only using this if you know the FP64 performance
        ''' of the GPU being used).
        ''' </summary>
        ''' <param name="amountOfNumbers">The amount of random numbers to generate.</param>
        ''' <returns>An IEnumerable holding the random numbers (in memory for the CPU to use).</returns>
        Public Function GenerateUniformDistributionDP(amountOfNumbers As Long) As ICudaResult(Of Double())
            Dim result(CInt(amountOfNumbers - 1)) As Double 'TEST Overflow? 
            Return GenerateUniformDistributionDP(amountOfNumbers, result)
        End Function

        Public Function GenerateLogNormalDistribution(amountOfNumbers As Long, result As Single(), mean As Single, stddev As Single) As ICudaResult(Of Single())
            Dim err = SafeNativeMethods.LogNormalRand(PtrToUnmanagedClass, result, amountOfNumbers, mean, stddev)
            Return New CudaResult(Of Single())(err, result)
        End Function

        ''' <summary>
        ''' Generates random numbers using XORWOW and a log normal distribution. This method utilizies the single-precision (FP32) capabilities of the GPU. If you need higher precision,
        ''' there is a double-precision (FP64) version available however, performance will be much worse, depending on the GPU being used.
        ''' </summary>
        ''' <param name="amountOfNumbers">The amount of random numbers to generate.</param>
        ''' <returns>An IEnumerable holding the random numbers (in memory for the CPU to use).</returns>
        Public Function GenerateLogNormalDistribution(amountOfNumbers As Long, mean As Single, stddev As Single) As ICudaResult(Of Single())
            Dim result(CInt(amountOfNumbers - 1)) As Single 'TEST Overflow? 
            Return GenerateLogNormalDistribution(amountOfNumbers, result, mean, stddev)
        End Function

        Public Function GenerateLogNormalDistributionDP(amountOfNumbers As Long, result As Double(), mean As Single, stddev As Single) As ICudaResult(Of Double())
            Dim err = SafeNativeMethods.LogNormalRandDouble(PtrToUnmanagedClass, result, amountOfNumbers, mean, stddev)
            Return New CudaResult(Of Double())(err, result)
        End Function

        ''' <summary>
        ''' Generate random numbers using XORWOW and a log normal distribution.
        ''' This method utilizes the double-precision (FP64) capabilities of the GPU this will perform worse than 
        ''' using the single-precision (FP32) capabilities, and much worse on GeForce versus Quadro and Tesla.
        ''' </summary>
        ''' <param name="amountOfNumbers">The amount of random numbers to generate.</param>
        ''' <param name="mean">The mean (average) of the distribution.</param>
        ''' <param name="stddev">The standard deviation of the distribution.</param>
        ''' <returns>An IEnumerable holding the random numbers (in memory for the CPU to use).</returns>
        ''' <remarks>
        ''' Recommend only using this if you know the FP64 performance of the GPU being used.
        ''' </remarks>
        Public Function GenerateLogNormalDistributionDP(amountOfNumbers As Long, mean As Single, stddev As Single) As ICudaResult(Of Double())
            Dim result(CInt(amountOfNumbers - 1)) As Double 'TEST Overflow?
            Return GenerateLogNormalDistributionDP(amountOfNumbers, result, mean, stddev)
        End Function

        Public Function GenerateNormalDistribution(amountOfNumbers As Long, result As Single()) As ICudaResult(Of Single())
            Dim err = SafeNativeMethods.NormalRand(PtrToUnmanagedClass, result, amountOfNumbers)
            Return New CudaResult(Of Single())(err, result)
        End Function

        ''' <summary>
        ''' Generates random numbers using XORWOW and a normal distribution.
        ''' This method utilizies the single-precision (FP32) capabilities of the GPU.
        ''' If you need higher precision, there is a double-precision (FP64) version available however, performance will be much worse, depending on the GPU being used.
        ''' </summary>
        ''' <param name="amountOfNumbers">The amount of random numbers to generate.</param>
        ''' <returns>An IEnumerable holding the random numbers (in memory for the CPU to use).</returns>
        Public Function GenerateNormalDistribution(amountOfNumbers As Long) As ICudaResult(Of Single())
            Dim result(CInt(amountOfNumbers - 1)) As Single 'TEST Overflow?
            Return GenerateNormalDistribution(amountOfNumbers, result)
        End Function

        Public Function GenerateNormalDistributionDP(amountOfNumbers As Long, result As Double()) As ICudaResult(Of Double())
            Dim err = SafeNativeMethods.NormalRandDouble(PtrToUnmanagedClass, result, amountOfNumbers)
            Return New CudaResult(Of Double())(err, result)
        End Function

        ''' <summary>
        ''' Generates random numbers using XORWOW and a normal distribution.
        ''' This method utilizes the double-precision (FP64) capabilities of the GPU this will perform worse than 
        ''' using the single-precision (FP32) capabilities, and much worse on GeForce versus Quadro and Tesla.
        ''' </summary>
        ''' <param name="amountOfNumbers">The amount of random numbers to generate.</param>
        ''' <returns>An IEnumerable holding the random numbers (in memory for the CPU to use).</returns>
        ''' <remarks>
        ''' Recommend only using this if you know the FP64 performance of the GPU being used.
        ''' </remarks>
        Public Function GenerateNormalDistributionDP(amountOfNumbers As Long) As ICudaResult(Of Double())
            Dim result(CInt(amountOfNumbers - 1)) As Double 'TEST Overflow?
            Return GenerateNormalDistributionDP(amountOfNumbers, result)
        End Function

        Public Function GeneratePoissonDistribution(amountOfNumbers As Long, result As Integer(), lambda As Double) As ICudaResult(Of Integer())
            Dim err = SafeNativeMethods.PoissonRand(PtrToUnmanagedClass, result, amountOfNumbers, lambda)
            Return New CudaResult(Of Integer())(err, result)
        End Function

        Public Function GeneratePoissonDistribution(amountOfNumbers As Long, lambda As Double) As ICudaResult(Of Integer())
            Dim result(CInt(amountOfNumbers - 1)) As Integer 'TEST Overflow?
            Return GeneratePoissonDistribution(amountOfNumbers, result, lambda)
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
                    SafeNativeMethods.DisposeRandomClass(PtrToUnmanagedClass)
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