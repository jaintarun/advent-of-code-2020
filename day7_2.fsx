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

let rec CountChildBags (map:Map<Color, Bag[]>) (color:Color):int =    
    match map.TryGetValue color with
    | true, [||] -> 0
    | true, bags -> 
        let childColors =
            bags   
            |> Array.map (fun (Bag(_, cnt, childBagColor)) -> (childBagColor, cnt))
                        
        childColors
        |> Array.map (fun (childBagColor, cnt) -> 
                        let childCount = CountChildBags map childBagColor
                        (childCount + 1) * cnt
                     )
        |> Array.sum        
    | _ -> 0

let groupedBags =
    File.ReadAllText("data\\day_7_data.txt").Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map ParseLine
    |> Array.reduce Array.append
    |> Array.groupBy (fun (Bag(parentBagColor, _, _)) -> parentBagColor)
    |> Map.ofArray

let answer = CountChildBags groupedBags (Color "shiny gold")

printfn $"{answer}"                    
