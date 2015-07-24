open System
open System.IO
open System.Net.Http
open System.Threading.Tasks
open FSharp.Configuration

let awaitTask (t: Task) = t |> Async.AwaitIAsyncResult |> Async.Ignore

let save (content: HttpContent) onto = 
    use filestream = new FileStream(onto, FileMode.Create)
    let stream = content.ReadAsStreamAsync().Result
    stream.CopyToAsync(filestream) |> awaitTask

let download (fromURL:Uri) onto = 
    use request = new HttpRequestMessage(HttpMethod.Get, fromURL)
    use client = new HttpClient()
    let resp = client.SendAsync(request).Result        
    if resp.IsSuccessStatusCode then
        save resp.Content onto |> ignore

type Config = AppSettings<"app.config">

[<EntryPoint>]
let main argv =     
    download Config.URL Config.FilePath
    Console.ReadLine |> ignore
    0 
