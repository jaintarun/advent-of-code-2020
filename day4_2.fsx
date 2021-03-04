open System
open System.IO
open System.Text.RegularExpressions

type Passport = {
    Byr: string option
    Iyr: string option
    Eyr: string option
    Hgt: string option
    Hcl: string option
    Ecl: string option
    Pid: string option
    Cid: string option
}

let ParseLine (passport:Passport) (line:string) = 
    let lineParts = line.Split(':')
    let fieldName = lineParts.[0]
    let fieldValue = lineParts.[1]
    match fieldName with
    | "byr" -> {passport with Byr = Some fieldValue}
    | "iyr" -> {passport with Iyr = Some fieldValue}
    | "eyr" -> {passport with Eyr = Some fieldValue}
    | "hgt" -> {passport with Hgt = Some fieldValue}
    | "hcl" -> {passport with Hcl = Some fieldValue}
    | "ecl" -> {passport with Ecl = Some fieldValue}
    | "pid" -> {passport with Pid = Some fieldValue}
    | "cid" -> {passport with Cid = Some fieldValue}
    | _ ->  failwith "unexpected fieldname"

let ParseLinesToPassport (record:string) =     
    let lines = record.Split([|" "; "\r\n"|], StringSplitOptions.RemoveEmptyEntries)    
    let emptyPassport = {Byr = None; Iyr = None; Eyr = None; Hgt = None; Hcl = None; Ecl = None; Pid = None; Cid = None}
    lines |> Array.fold ParseLine emptyPassport    

let IsYearInvalid startYear endYear (year:string) = 
    match Int32.TryParse(year) with
    | (true,intYear) -> intYear >= startYear && intYear <= endYear
    | _ -> false

let CheckByr (passport: Passport) = 
    passport.Byr 
    |> Option.bind (fun byr -> if IsYearInvalid 1920 2002 byr then Some passport else None)

let CheckIyr (passport: Passport) = 
    passport.Iyr 
    |> Option.bind (fun iyr -> if IsYearInvalid 2010 2020 iyr then Some passport else None)    

let CheckEyr (passport: Passport) = 
    passport.Eyr 
    |> Option.bind (fun eyr -> if IsYearInvalid 2020 2030 eyr then Some passport else None)        

let ParseHgt hgt = 
    let m = Regex.Match(hgt,"(.*?)(cm|in)")

    let vStr =  m.Groups.[1].Value
    let uStr =  m.Groups.[2].Value

    let v = match Int32.TryParse(vStr) with
                    | (true, vInt) -> Some vInt
                    | _ -> None

    let u = match uStr with
                    | "cm" -> Some uStr
                    | "in" -> Some uStr
                    | _ -> None

    match (v, u) with
        | (Some vVal, Some uVal) when uVal = "cm" && vVal >= 150 && vVal <= 193 -> true
        | (Some vVal, Some uVal) when uVal = "in" && vVal >= 59 && vVal <= 76 -> true
        | _ -> false

let CheckHgt (passport:Passport) = 
    passport.Hgt 
    |> Option.bind (fun hgt -> if ParseHgt hgt then Some passport else None)            

let CheckHcl (passport:Passport) = 
    passport.Hcl 
    |> Option.bind (fun hcl -> if Regex.IsMatch(hcl,"^#[0-9a-z]{6}$") then Some passport else None)                

let CheckEcl (passport:Passport) = 
    passport.Ecl 
    |> Option.bind (fun ecl -> if Regex.IsMatch(ecl,"(amb|blu|brn|gry|grn|hzl|oth)") then Some passport else None)                    

let CheckPid (passport:Passport) = 
    passport.Pid 
    |> Option.bind (fun pid -> if Regex.IsMatch(pid,"^[0-9]{9}$") then Some passport else None)                    

let IsPassportValid (passport:Passport) = 
    passport
    |> CheckByr
    |> Option.bind CheckIyr
    |> Option.bind CheckEyr
    |> Option.bind CheckHgt
    |> Option.bind CheckHcl
    |> Option.bind CheckEcl
    |> Option.bind CheckPid
    |> Option.isSome
  
let answer = File.ReadAllText("data\\day_4_data.txt").Split("\r\n\r\n", StringSplitOptions.RemoveEmptyEntries)
                 |> Array.map ParseLinesToPassport
                 |> Array.filter IsPassportValid
                 |> Array.length
                 
printfn $"{answer}"