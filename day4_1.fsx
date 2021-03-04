open System
open System.IO

type Passport = { 
    byr: string option
    iyr: string option
    eyr: string option
    hgt: string option
    hcl: string option
    ecl: string option
    pid: string option
    cid: string option
}

let ParseLine (passport:Passport) (line:string) = 
    let lineParts = line.Split(':')
    let fieldName = lineParts.[0]
    let fieldValue = lineParts.[1]

    match fieldName with
    | "byr" -> {passport with byr = Some fieldValue}
    | "iyr" -> {passport with iyr = Some fieldValue}
    | "eyr" -> {passport with eyr = Some fieldValue}
    | "hgt" -> {passport with hgt = Some fieldValue}
    | "hcl" -> {passport with hcl = Some fieldValue}
    | "ecl" -> {passport with ecl = Some fieldValue}
    | "pid" -> {passport with pid = Some fieldValue}
    | "cid" -> {passport with cid = Some fieldValue}
    | _ ->  passport

let ParseLinesToPassport (record:string) =     
    let lines = record.Split([|" "; "\r\n"|], StringSplitOptions.RemoveEmptyEntries)
    let emptyPassport = {byr = None; iyr = None; eyr = None; hgt = None; hcl = None; ecl = None; pid = None; cid = None}    
    lines |> Array.fold ParseLine emptyPassport    

let IsPassportValid (passport:Passport) = 
    match passport with
    | {byr = None} -> false
    | {iyr = None} -> false
    | {eyr = None} -> false
    | {hgt = None} -> false
    | {hcl = None} -> false
    | {ecl = None} -> false
    | {pid = None} -> false
    | _ -> true
  
let answer = File.ReadAllText("data\\day_4_data.txt").Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
                 |> Array.map ParseLinesToPassport
                 |> Array.filter IsPassportValid
                 |> Array.length
                 
printfn $"{answer}"