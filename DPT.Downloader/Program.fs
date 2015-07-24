// Más información acerca de F# en http://fsharp.net
// Vea el proyecto "Tutorial de F#" para obtener más ayuda.

open System
open System.IO
open System.Net.Http
open System.Threading.Tasks

exception Error1 of string

let awaitTask (t: Task) = t |> Async.AwaitIAsyncResult |> Async.Ignore

let save (content: HttpContent) onto = 
    use filestream = new FileStream(onto, FileMode.Create)
    let stream = content.ReadAsStreamAsync().Result
    stream.CopyToAsync(filestream) |> awaitTask

let download (fromURL:string) onto = 
    use request = new HttpRequestMessage(HttpMethod.Get, fromURL)
    use client = new HttpClient()
    let resp = client.SendAsync(request).Result        
    if resp.IsSuccessStatusCode then
        save resp.Content onto |> ignore

let URL = "http://www.diprotech.com/public/Configuration.xml"
let FILE = @"C:\tmp\jaume\el_fichero.xml"
[<EntryPoint>]
let main argv = 
    download URL FILE
    Console.ReadLine |> ignore
    0 // devolver un código de salida entero
