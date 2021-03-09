open System
open System.IO

let GetUniqueCount (line:string) =     
    line.ToCharArray() |> Set.ofSeq |> Set.count

let answer = File.ReadAllText("data\\day_6_data.txt").Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
                |> Array.map (fun s -> s.Replace("\r\n", ""))
                |> Array.map GetUniqueCount
                |> Array.sum   
                 
printfn $"{answer}"                 
