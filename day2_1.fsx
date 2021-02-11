open System.IO

type PasswordPolicy = {
    min: int;
    max: int;
    letter: char;
    password: string
}

let parseData (line: string) = 
    let z = line.Replace("-", " ").Replace(":", "").Split(" ")
    { min= int z.[0]; max= int z.[1]; letter=char z.[2]; password= z.[3] }

let checkPasswordPolicy policy =     
    let charCount = policy.password 
                        |> String.filter (fun c -> c = policy.letter)
                        |> String.length

    charCount >= policy.min && charCount <= policy.max

let data = File.ReadLines("data\\day_2_data.txt") 
            |> Array.ofSeq
            |> Array.map parseData
            |> Array.filter checkPasswordPolicy
            |> Array.length

printfn $"{data}"