# Picture Puzzle

A Windows 11 desktop picture puzzle. Choose a sample image or your own picture, then rearrange the tiles until the image is complete.

## Features

- Choose a sample picture or load a PNG, JPG, BMP, GIF, or TIFF image.
- Play on a 3 × 3, 4 × 4, or 5 × 5 board.
- Swap two horizontally or vertically adjacent tiles by selecting them or dragging one onto its neighbor. Non-adjacent tiles cannot be swapped.
- Track moves and elapsed time.
- Open the full-picture hint in a separate window without cropping the image.
- Switch between Indonesian and English, and between light and dark themes.

## Run on Windows

Run `SusunGambar.exe` from the project folder, or use the standalone build at `bin\Release\net10.0-windows\win-x64\publish\SusunGambar.exe`. The published Windows x64 application is self-contained and does not require a separate .NET installation.

To rebuild the standalone application, install the .NET 10 SDK and run:

```powershell
dotnet publish .\SusunGambar.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

To run the project during development, use `dotnet run`.

## Share the game

For players, distribute `SusunGambar.exe` on its own, or put it in a ZIP file. No source files or additional runtime files are needed for the published Windows x64 build. You can upload the EXE or ZIP as an asset on a GitHub Release. The executable is intentionally excluded from Git history because of its size.

## AI creation and development

All game-specific source code in this project was generated and implemented by an AI assistant; the user did not write the application code manually. The human contribution was to propose the game idea, describe and refine desired features, choose Indonesian and English as the supported languages, try the application, and provide feedback and screenshots that guided fixes.

Development took place through **GitHub Copilot in Visual Studio Code**, using an AI assistant accessed via the Copilot SDK. The assistant helped create and update the C# and WPF/XAML application, implement the puzzle and interface, diagnose build issues, and publish and smoke-test the Windows executable.

**Model disclosure:** The exact underlying model name and version used for each coding interaction are not available in the project’s records, so this README does not guess or attribute a specific model. The project targets **.NET 10** and uses **WPF**; these are the application’s development technologies, not the name of the AI model.

---

# Susun Gambar

Game puzzle gambar desktop untuk Windows 11. Pilih gambar contoh atau gambar milikmu, lalu susun kembali potongannya hingga gambar utuh.

## Fitur

- Gunakan gambar contoh atau pilih gambar PNG, JPG, BMP, GIF, atau TIFF.
- Pilih papan 3 × 3, 4 × 4, atau 5 × 5.
- Tukar dua potongan yang berdampingan secara horizontal atau vertikal dengan memilih keduanya atau menyeret satu ke tetangganya. Potongan yang tidak berdampingan tidak bisa ditukar.
- Catat jumlah langkah dan waktu bermain.
- Buka petunjuk gambar utuh di jendela terpisah tanpa memotong gambar.
- Ganti bahasa antara Bahasa Indonesia dan English, serta tema terang dan gelap.

## Menjalankan di Windows

Jalankan `SusunGambar.exe` dari folder project, atau gunakan aplikasi mandiri di `bin\Release\net10.0-windows\win-x64\publish\SusunGambar.exe`. Aplikasi Windows x64 hasil publish sudah menyertakan runtime dan tidak memerlukan instalasi .NET terpisah.

Untuk membangun ulang aplikasi mandiri, pasang .NET 10 SDK lalu jalankan:

```powershell
dotnet publish .\SusunGambar.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

Untuk menjalankan project selama pengembangan, gunakan `dotnet run`.

## Membagikan game

Untuk pemain, bagikan file `SusunGambar.exe` saja atau masukkan file tersebut ke dalam ZIP. Build Windows x64 hasil publish tidak memerlukan file source maupun file runtime tambahan. EXE atau ZIP dapat diunggah sebagai aset GitHub Release. File EXE sengaja dikecualikan dari riwayat Git karena ukurannya besar.

## Pembuatan dan pengembangan dengan AI

Seluruh kode sumber khusus game ini dibuat dan diimplementasikan oleh asisten AI; pengguna tidak menulis kode aplikasi secara manual. Kontribusi manusia adalah mengusulkan ide game, menjelaskan dan menyempurnakan fitur yang diinginkan, memilih Bahasa Indonesia dan English sebagai bahasa yang didukung, mencoba aplikasi, serta memberikan masukan dan screenshot yang menjadi panduan perbaikan.

Pengembangan dilakukan menggunakan **GitHub Copilot di Visual Studio Code**, dengan asisten AI yang diakses melalui Copilot SDK. Asisten membantu membuat dan memperbarui aplikasi C# dan WPF/XAML, mengimplementasikan puzzle dan antarmukanya, mendiagnosis masalah build, serta memublikasikan dan menguji aplikasi Windows.

**Keterangan model:** Nama dan versi pasti model yang mendasari setiap interaksi coding tidak tersedia dalam catatan project ini. Karena itu, README ini tidak menebak atau mengklaim model tertentu. Project menargetkan **.NET 10** dan menggunakan **WPF**; keduanya adalah teknologi pengembangan aplikasi, bukan nama model AI.
