# Susun Gambar

Game puzzle gambar desktop untuk Windows 11. Susun potongan dengan mengeklik dua potongan untuk menukar posisinya, atau dengan menarik satu potongan ke potongan lain.

## Menjalankan di Windows

Jalankan `bin\Release\net10.0-windows\win-x64\publish\SusunGambar.exe`. Versi ini mandiri dan tidak memerlukan instalasi .NET.

Untuk membuat ulang file aplikasi mandiri, pasang .NET 10 SDK lalu jalankan:

```powershell
dotnet publish .\SusunGambar.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

Untuk menjalankan project selama pengembangan, gunakan `dotnet run`.

## Cara bermain

Mulai dengan gambar contoh atau pilih gambar PNG, JPG, BMP, GIF, atau TIFF dari komputer. Pilih tingkat kesulitan 3 × 3, 4 × 4, atau 5 × 5. Semua kotak selalu berisi potongan. Klik satu potongan lalu klik potongan di sebelahnya untuk menukar posisi, atau seret potongan ke tetangga di atas, bawah, kiri, atau kanan. Potongan yang tidak berdampingan tidak bisa ditukar. Game mencatat langkah dan waktu. Tombol **Lihat gambar utuh** membuka petunjuk dalam jendela tersendiri yang menjaga rasio gambar agar seluruh gambar terlihat. Gunakan pilihan bahasa di kanan atas untuk beralih antara Bahasa Indonesia dan English. Gunakan tombol mode untuk mengganti tema terang/gelap.

## Membagikan game

Untuk pengguna Windows 11 x64, cukup bagikan file `SusunGambar.exe` yang dibuat dengan perintah publish di atas. File ini mandiri dan tidak memerlukan instalasi .NET. ZIP opsional hanya perlu berisi `SusunGambar.exe` dan, bila diinginkan, petunjuk singkat `README.txt`. Unggah ZIP atau EXE sebagai aset GitHub Release; jangan masukkan EXE besar ke riwayat source Git.
