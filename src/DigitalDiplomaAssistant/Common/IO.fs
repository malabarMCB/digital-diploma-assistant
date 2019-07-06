namespace Common

open System.IO

module IO =
    let getFileStream basePath filePath = 
        let path = basePath + @"\" + filePath
        File.OpenRead path
