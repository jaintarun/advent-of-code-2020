open System
open System.IO
open System.Text.RegularExpressions

type Seat =
    { Row: char list
      Col: char list }

let ParseLineToSeat (line: string) =
    let m = Regex.Match(line, "^(.{7})(.{3})$")
    let rowStr = m.Groups.[1].Value
    let colStr = m.Groups.[2].Value

    { Row = rowStr |> Seq.toList 
      Col = colStr |> Seq.toList  }

let rec RowDecoder (l: int, u: int) (rows: char list) =
    match rows with
    | [ lastOne ] ->
        match lastOne with
        | 'F' -> l
        | 'B' -> u
    | h :: t ->
        match h with
        | 'F' -> RowDecoder(l, (u + l - 1) / 2) t
        | 'B' -> RowDecoder((u + l + 1) / 2, u) t

let rec ColumnDecoder (l: int, u: int) (cols: char list) =
    match cols with
    | [ lastOne ] ->
        match lastOne with
        | 'L' -> l
        | 'R' -> u
    | h :: t ->
        match h with
        | 'L' -> ColumnDecoder(l, (u + l - 1) / 2) t
        | 'R' -> ColumnDecoder((u + l + 1) / 2, u) t

let CalcSeatId (seat: Seat) =
    let column = seat.Col |> ColumnDecoder(0, 7)
    let row = seat.Row |> RowDecoder(0, 127)
    row * 8 + column

let answer =
    File
        .ReadAllText("data\\day_5_data.txt")
        .Split("\r\n", StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (ParseLineToSeat >> CalcSeatId)
    |> Array.max
