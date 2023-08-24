Imports System.Runtime.InteropServices

Namespace CudaVber

    ''' <summary>
    ''' These call directly to methods specified in CudaSharperLibrary.dll.
    ''' The types used here must be CLS-compliant and they must be specific.
    ''' They must align with what is defined in the C++ code e.g., Long _t must be mirrored with Long.
    ''' These methods are as raw as possible - any sort of type translation should be done in DTM.
    ''' </summary>
    ''' <example>
    ''' Example: CUDA provides cudaError_t, which is an error code that each function returns.
    ''' It is an int enum.
    ''' Enums are not blittable, but ints are.
    ''' The C++ code takes the cudaError_t, casts it to an int, and returns it.
    ''' This is why all of these methods have an int return value. 
    ''' DTM is where that int is then cast back to an enum, which is defined in C# as CudaError.
    ''' </example>
    ''' <remarks>
    ''' Rules for adding a method:
    ''' <list type="bullet">
    ''' <item>DO use Integer instead of int and Long instead of long.</item>
    ''' <item>DO make ALL methods return Integer (and no tuples).</item>
    ''' <item>DO make ALL methods friend (absolutely no public methods).</item>
    ''' <item>DO make device_id the first parameter.</item>
    ''' <item>DO make device_id Integer.</item>
    ''' <item>DO make the size of the array follow the array (e.g., Method(Single() arr, Integer arr_size)).</item>
    ''' <item>DO not use uint, ulong, etc.</item>
    ''' <item>DO not generics.</item>
    ''' <item>DO not use Int16 or byte/sbyte.</item>
    ''' </list>
    ''' </remarks>
    Friend Module SafeNativeMethods

        Friend Const LIB_NAME As String = "CudaSharperLibrary.dll"

        <DllImport(LIB_NAME)>
        Friend Function CreateRandomClass(device_id As Integer, amount_of_numbers As Long) As IntPtr
        End Function

        <DllImport(LIB_NAME)>
        Friend Sub DisposeRandomClass(cuda_rand As IntPtr)
        End Sub

        <DllImport(LIB_NAME)>
        Friend Function CreateArrayClass(device_id As Integer, amount_of_numbers As Long) As IntPtr
        End Function

        <DllImport(LIB_NAME)>
        Friend Sub DisposeArrayClass(cuda_rand As IntPtr)
        End Sub

        <DllImport(LIB_NAME)>
        Friend Function CreateStatClass(device_id As Integer, amount_of_numbers As Long) As IntPtr
        End Function

        <DllImport(LIB_NAME)>
        Friend Sub DisposeStatClass(cuda_rand As IntPtr)
        End Sub

#Region "CudaDevice"

        <DllImport(LIB_NAME, CallingConvention:=CallingConvention.Cdecl, CharSet:=CharSet.Ansi, BestFitMapping:=False, ThrowOnUnmappableChar:=True)>
        Friend Function GetCudaDeviceName(device_id As Integer, device_name_ptr As Text.StringBuilder) As Integer
        End Function

#End Region

#Region "CudaSettings"

        'Private Const NvRtcLibName As String = "nvrtc64_112_0"
        '<DllImport(NvRtcLibName, CallingConvention:=CallingConvention.Cdecl)>
        'Private Function nvrtcCreateProgram(ByRef program As ULong, sourceCode As String, fileName As String, numHeaders As Integer, ByRef headers As ULong, ByRef includeNames As ULong) As NvrtcResult
        'End Function

        <DllImport(LIB_NAME, EntryPoint:="GetCudaDeviceCount")>
        Public Function GetCudaDeviceCount() As Int32
        End Function

        <DllImport(LIB_NAME, EntryPoint:="ResetCudaDevice")>
        Friend Function ResetCudaDevice() As Integer
        End Function

#End Region

#Region "CuRand"

        <DllImport(LIB_NAME)>
        Friend Function UniformRand(cuda_rand As IntPtr, result As Single(), amount_of_numbers As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function UniformRandDouble(cuda_rand As IntPtr, result As Double(), amount_of_numbers As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function LogNormalRand(cuda_rand As IntPtr, result As Single(), amount_of_numbers As Long, mean As Single, stddev As Single) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function LogNormalRandDouble(cuda_rand As IntPtr, result As Double(), amount_of_numbers As Long, mean As Single, stddev As Single) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function NormalRand(cuda_rand As IntPtr, result As Single(), amount_of_numbers As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function NormalRandDouble(cuda_rand As IntPtr, result As Double(), amount_of_numbers As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function PoissonRand(cuda_rand As IntPtr, result As Integer(), amount_of_numbers As Long, lambda As Double) As Integer
        End Function

#End Region

#Region "CuArray"

        <DllImport(LIB_NAME)>
        Friend Function AddIntArrays(cuarray As IntPtr, result As Integer(), array1 As Integer(), array2 As Integer(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function AddLongArrays(cuarray As IntPtr, result As Long(), array1 As Long(), array2 As Long(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function AddFloatArrays(cuarray As IntPtr, result As Single(), array1 As Single(), array2 As Single(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function AddDoubleArrays(cuarray As IntPtr, result As Double(), array1 As Double(), array2 As Double(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function SubtractIntArrays(cuarray As IntPtr, result As Integer(), array1 As Integer(), array2 As Integer(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function SubtractFloatArrays(cuarray As IntPtr, result As Single(), array1 As Single(), array2 As Single(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function SubtractLongArrays(cuarray As IntPtr, result As Long(), array1 As Long(), array2 As Long(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function SubtractDoubleArrays(cuarray As IntPtr, result As Double(), array1 As Double(), array2 As Double(), length As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function MatrixMultiplyFloat(device_id As Integer, transa_op As Integer, transb_op As Integer, m As Integer, n As Integer, k As Integer,
                                             alpha As Single, a As Single(), b As Single(), beta As Single, c As Single()) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function MatrixMultiplyDouble(device_id As Integer, transa_op As Integer, transb_op As Integer, m As Integer, n As Integer, k As Integer,
                                             alpha As Double, a As Double(), b As Double(), beta As Double, c As Double()) As Integer
        End Function

#End Region

#Region "cuStats"

        <DllImport(LIB_NAME)>
        Friend Function SampleStandardDeviationFloat(custat As IntPtr, ByRef result As Double, sample As Single(), sample_size As Long, mean As Single) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function SampleStandardDeviationDouble(custat As IntPtr, ByRef result As Double, sample As Double(), sample_size As Long, mean As Double) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function StandardDeviationFloat(custat As IntPtr, ByRef result As Double, sample As Single(), sample_size As Long, mean As Single) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function StandardDeviationDouble(custat As IntPtr, ByRef result As Double, sample As Double(), sample_size As Long, mean As Double) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function SampleCovarianceFloat(custat As IntPtr, ByRef result As Double, x_array As Single(), x_mean As Single, y_array As Single(), y_mean As Single, sample_size As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function SampleCovarianceDouble(custat As IntPtr, ByRef result As Double, x_array As Double(), x_mean As Double, y_array As Double(), y_mean As Double, sample_size As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function CovarianceFloat(custat As IntPtr, ByRef result As Double, x_array As Single(), x_mean As Single, y_array As Single(), y_mean As Single, sample_size As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function CovarianceDouble(custat As IntPtr, ByRef result As Double, x_array As Double(), x_mean As Double, y_array As Double(), y_mean As Double, sample_size As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function PearsonCorrelationFloat(custat As IntPtr, ByRef result As Double, x_array As Single(), x_mean As Single, y_array As Single(), y_mean As Single, sample_size As Long) As Integer
        End Function

        <DllImport(LIB_NAME)>
        Friend Function PearsonCorrelationDouble(custat As IntPtr, ByRef result As Double, x_array As Double(), x_mean As Double, y_array As Double(), y_mean As Double, sample_size As Long) As Integer
        End Function

#End Region

    End Module

End Namespace