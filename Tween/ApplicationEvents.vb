Namespace My

    ' 次のイベントは MyApplication に対して利用できます:
    ' 
    ' Startup: アプリケーションが開始されたとき、スタートアップ フォームが作成される前に発生します。
    ' Shutdown: アプリケーション フォームがすべて閉じられた後に発生します。このイベントは、通常の終了以外の方法でアプリケーションが終了されたときには発生しません。
    ' UnhandledException: ハンドルされていない例外がアプリケーションで発生したときに発生するイベントです。
    ' StartupNextInstance: 単一インスタンス アプリケーションが起動され、それが既にアクティブであるときに発生します。 
    ' NetworkAvailabilityChanged: ネットワーク接続が接続されたとき、または切断されたときに発生します。
    Partial Friend Class MyApplication
        Private mt As System.Threading.Mutex

        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown
            Try
                mt.ReleaseMutex()
                mt.Close()
            Catch ex As Exception

            End Try
        End Sub

        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Dim pt As String = Application.Info.DirectoryPath.Replace("\", "/") + "/" + Application.Info.ProductName
            mt = New System.Threading.Mutex(False, pt)
            If mt.WaitOne(0, False) = False Then
                MessageBox.Show("Tweenは既に起動されています。2重起動する場合は、別フォルダのTween.exeを実行してください。", "Tween二重起動チェック", MessageBoxButtons.OK, MessageBoxIcon.Information)
                e.Cancel = True
            Else
                GC.KeepAlive(mt)
            End If
        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            My.Application.Log.DefaultFileLogWriter.Location = Logging.LogFileLocation.ExecutableDirectory
            My.Application.Log.DefaultFileLogWriter.MaxFileSize = 102400
            My.Application.Log.DefaultFileLogWriter.AutoFlush = True
            My.Application.Log.DefaultFileLogWriter.Append = False
            'My.Application.Log.WriteException(e.Exception, _
            '    Diagnostics.TraceEventType.Critical, _
            '    "Source=" + e.Exception.Source + " StackTrace=" + e.Exception.StackTrace + " InnerException=" + IIf(e.Exception.InnerException Is Nothing, "", e.Exception.InnerException.Message))
            My.Application.Log.WriteException(e.Exception, _
                Diagnostics.TraceEventType.Critical, _
                e.Exception.StackTrace + vbCrLf + Now.ToString)
            MessageBox.Show("エラーが発生しました。ごめんなさい。ログをexeファイルのある場所にTween.logとして作ったので、kiri.feather@gmail.comまで送っていただけると助かります。ご面倒なら@kiri_featherまでお知らせ頂くだけでも助かります。", "エラー発生", MessageBoxButtons.OK, MessageBoxIcon.Error)
            e.ExitApplication = False
        End Sub
    End Class

End Namespace

