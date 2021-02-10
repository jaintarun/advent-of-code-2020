open System.IO

type CoordinateType =
    | OpenSquare
    | Tree

type GridRow = { Cells: CoordinateType[] }    
type Grid = { Rows: GridRow[] }


let parseData (line: string) = 
    let z = line.Replace("-", " ").Replace(":", "").Split(" ")
    { min= int z.[0]; max= int z.[1]; letter=char z.[2]; password= z.[3] }

let data = File.ReadLines("day_3_data.txt") 
            |> Array.ofSeq
            |> Array.map parseData