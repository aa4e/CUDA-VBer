Imports CudaVber

Module Module1

    Sub Main()

        Try
            Console.WriteLine("CUDA SHARPER LIBRARY ON VBNET")

            ' Load the DLL. It only has to be called once.
            CudaSettings.Load()
            Console.WriteLine("Library loaded")

            ' We'll use the first CUDA-enabled GPU (this system has a GTX 1070 [which is 0] and a GTX 1050 Ti [which is 1]).
            Dim sw As New Stopwatch()
            sw.Start()

            Dim uniform_rand As ICudaResult(Of Single())
            Dim num As Integer = 10000000
            Console.WriteLine($"Generate {num} random numbers using a uniform distribution...")
            Using cudaObject As New CuRand(New CudaDevice(0, num))
                uniform_rand = cudaObject.GenerateUniformDistribution(num)
            End Using
            Console.WriteLine($"Elapsed {sw.Elapsed.TotalMilliseconds} ms")

            Using fs As New IO.FileStream("uni_rand.txt", IO.FileMode.Create), w As New IO.StreamWriter(fs)
                For Each s In uniform_rand.Result
                    w.WriteLine(s)
                Next
                Console.WriteLine($"Saved: {fs.Name}")
            End Using

        Catch ex As Exception
            Console.ForegroundColor = ConsoleColor.Red
            Console.WriteLine(ex)
        End Try

        Console.ReadKey()

    End Sub

End Module
