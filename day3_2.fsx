open System.IO

type CoordinateType =
    | Open
    | Tree

let parseLetterToCoordinateType c = 
    match c with
    | '.' -> Open
    | '#' -> Tree
    | _ -> failwith "Bug in code or input data!"

let parseToGridRow (line: string) = 
    line.ToCharArray() 
        |> Array.map parseLetterToCoordinateType

let getCoordinateType x i gridRow : CoordinateType =
    let newX = (i * x) % Array.length gridRow
    gridRow.[newX]    

let RowChooser y data =
    (fst data) % y = 0

let CalculateTreeForStep (inputData:CoordinateType[][]) (x:int, y:int) =
    inputData 
        |> Array.indexed
        |> Array.filter (RowChooser y)
        |> Array.map (fun row -> (snd row))
        |> Array.mapi (getCoordinateType x)
        |> Array.filter (fun element -> element = Tree)
        |> Array.length
   
let inputData = File.ReadLines("data\\day_3_data.txt")
                |> Seq.toArray
                |> Array.map parseToGridRow

let inputSteps = [| (1, 1); (3, 1); (5, 1); (7, 1); (1, 2) |]

let answer = (Array.map ((CalculateTreeForStep inputData) >> (fun x -> int64(x))) inputSteps)
            |> Array.reduce (*)

printfn $"******************** {answer} ***********************"