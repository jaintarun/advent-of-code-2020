open System.IO

type CoordinateType =
    | Open
    | Tree

let parseLetterToCoordinateType (c: char) = 
    match c with
    | '.' -> Open
    | '#' -> Tree
    | _ -> failwith "a bug in the code!"

let parseToGridRow (line: string) = 
    line.ToCharArray() 
        |> Array.map parseLetterToCoordinateType

let getCoordinateType i gridRow : CoordinateType =
    let newX = (i * 3) % Array.length gridRow
    gridRow.[newX]    
   
let inputData = File.ReadLines("day_3_data.txt")  
                |> Array.ofSeq 
                |> Array.map parseToGridRow

let answer = inputData 
                |> Array.mapi getCoordinateType
                |> Array.filter (fun element -> element = Tree)
                |> Array.length

printfn $"{answer}"