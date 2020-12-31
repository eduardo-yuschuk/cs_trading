(*
Copyright 2014 Eduardo Yuschuk (eduardo.yuschuk@gmail.com)
*)

// Más información acerca de F# en http://fsharp.org
// Vea el proyecto "Tutorial de F#" para obtener más ayuda.
open System
open System.IO
open System.Globalization

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
}

let textPricesPath = @"D:\400 DATA\EURUSD_DUKAS_TICKS.txt"
let epoch = new DateTime(1970, 1, 1)

let parse (line:string) =
    let parts = line.Split([|','|])
    let time = epoch + TimeSpan.FromMilliseconds(float(Int64.Parse(parts.[0])))
    let ask = Decimal.Parse(parts.[1].Replace(".", ","))
    (time, ask)
    
let samples = 
    readLines textPricesPath
    |> Seq.map parse
    |> Seq.filter (fun (time, ask) -> time.Year = 2014)

samples
|> Seq.iter (fun (time, ask) -> printfn "%s" (time.ToString ()))



[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // devolver un código de salida entero
