Namespace CudaVber

    ''' <summary>
    ''' Initialization Class.
    ''' </summary>
    ''' <remarks>
    ''' CudaSharper - a wrapper for CUDA-accelerated functions.
    ''' CudaSharper is not intended to write CUDA in C#, but rather a library that allows one to easily use CUDA-accelerated functions without having to directly interact with the device.
    ''' This file acts as a wrapper for CudaSharperLibrary.dll, which is required for these functions to run. 
    ''' CudaSharperLibrary.dll is the actual CUDA C code compiled as a C++/CLI assembly however, it is unmanaged and therefore requires this wrapper to be used in C# projects.
    ''' 
    ''' Current Classes:
    ''' <list type="bullet">
    ''' <item><see cref="CudaSettings"/>: Initialization Class.</item>
    ''' <item><see cref="CuArray"/>: Array functions, such as SplitArray and AddArrays</item>
    ''' <item><see cref="CuRand"/>: cuRAND functions allows the generation Of pseudorandom number sets using uniform, normal, or poisson distributions.</item>
    ''' </list>
    ''' </remarks>
    Public Class CudaSettings
        Implements IDisposable

        Private Shared Property WorkingDirSet As Boolean = False
        Private Shared Property LoadingLock As New Object()

        Public Shared ReadOnly Property CudaDeviceCount As Integer
        Public Shared ReadOnly Property Version As String = "v0.2.1"

        Public Shared Sub Load(workingDirectory As String)
            SyncLock LoadingLock

                CheckDll()

                If WorkingDirSet Then
                    Return
                End If

                Environment.SetEnvironmentVariable("PATH", System.Environment.GetEnvironmentVariable("PATH") & workingDirectory, EnvironmentVariableTarget.Process)
                Try
#If DEBUG Then
                    'Dim sb As New Text.StringBuilder()
                    'Dim n = SafeNativeMethods.GetCudaDeviceName(0, sb)
#End If


                    _CudaDeviceCount = SafeNativeMethods.GetCudaDeviceCount()
                    WorkingDirSet = True
                Catch e As DllNotFoundException
                    Console.WriteLine(Environment.GetEnvironmentVariable("PATH"))
                    Console.WriteLine(e.Message)
                    Throw
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            End SyncLock
        End Sub

        Public Shared Sub Load()
            Load(AppDomain.CurrentDomain.BaseDirectory)
        End Sub

        Private Shared Sub CheckDll()
            Dim dlls As New Dictionary(Of String, Byte()) From {
                {LIB_NAME, My.Resources.CudaSharperLib_dll}
            }
            For Each dll In dlls
                If (Not IO.File.Exists(dll.Key)) Then
                    Using compressed As New IO.MemoryStream(dll.Value),
                        gzStm As New IO.Compression.GZipStream(compressed, IO.Compression.CompressionMode.Decompress),
                        decompressed As IO.FileStream = IO.File.Create(dll.Key)

                        gzStm.CopyTo(decompressed)

                    End Using
                End If
            Next
        End Sub

#Region "IDISPOSABLE"

        Private DisposedValue As Boolean

        Protected Overridable Sub Dispose(disposing As Boolean)
            If (Not DisposedValue) Then
                If disposing Then
                    'dispose managed state (managed objects)
                End If

                DTM.ResetCudaDevice()
                'SafeNativeMethods.cuStats_Dispose();

                'free unmanaged resources (unmanaged objects) and override finalizer, set large fields to null
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