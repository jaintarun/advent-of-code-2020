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

let checkNewPasswordPolicy policy = 
    match policy.password.Length < policy.max with
        | true -> policy.password.[policy.min-1] = policy.letter
        | false -> (policy.password.[policy.min-1] = policy.letter) <> (policy.password.[policy.max-1] = policy.letter)

//let data = checkPasswordPolicy_new {min=1;max=3;letter='b';password="cdefg"}       

let count = File.ReadLines("data\\day_2_data.txt") 
            |> Array.ofSeq
            |> Array.map parseData
            |> Array.filter checkNewPasswordPolicy
            |> Array.length

printfn $"{count}"