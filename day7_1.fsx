open System
open System.IO
open System.Text.RegularExpressions

type Color = Color of string

type Bag = 
    | Bag of ParentColor:Color * ChildCnt:int * ChildBagColor:Color

let ParseLine (line:string) =
    let m = Regex.Match(line, "^(.*?) bags contain (.*).")
    let parentBagColor = m.Groups.[1].Value |> Color
    
    let children = m.Groups.[2].Value.Split(",", StringSplitOptions.RemoveEmptyEntries)
    let parsedChildren = children
                            |> Array.choose (fun x -> 
                                                let z = Regex.Match(x.Trim(), "^(.*?) (.*?) bag*")
                                                
                                                match Int32.TryParse z.Groups.[1].Value with
                                                | true, num -> 
                                                    Some (Bag(parentBagColor, num, Color z.Groups.[2].Value))
                                                | _ -> None
                                         )

    parsedChildren

let rec CountBags (map:Map<Color, Bag[]>) (color:Color): Color array =
    match map.TryGetValue color with
    | true, [||] -> [||]
    | true, bags -> 
        let parentColors =
            bags   
            |> Array.map (fun (Bag(parentColor, _, _)) -> parentColor)
                        
        let higherParents =
            parentColors
            |> Array.collect (CountBags map)

        Array.append parentColors higherParents
    | _ -> [||]


let groupedBags =
    File.ReadAllText("data\\day_7_data.txt").Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map ParseLine
    |> Array.reduce Array.append
    |> Array.groupBy (fun (Bag(_, _, childBagColor)) -> childBagColor)
    |> Map.ofArray

let answer =
    CountBags groupedBags (Color "shiny gold")        
    |> Array.distinct
    |> Array.length

printfn $"{answer}"                    
