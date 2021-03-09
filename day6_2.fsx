open System
open System.IO

let GetCountOfAllYes (text:string) =
    text.Split("\r\n", StringSplitOptions.RemoveEmptyEntries) 
        |> List.ofSeq
        |> List.map (fun (line:string) -> line.ToCharArray() |> Set.ofSeq)
        |> Set.intersectMany
        |> Set.count

let answer = File.ReadAllText("data\\day_6_data.txt").Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
                |> Array.map GetCountOfAllYes
                |> Array.sum

printfn $"{answer}"
