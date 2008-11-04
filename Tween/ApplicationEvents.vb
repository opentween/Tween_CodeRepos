Option Strict On
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
            Dim now As DateTime = DateTime.Now
            Dim fileName As String = String.Format("Tween-{0:0000}{1:00}{2:00}-{3:00}{4:00}{5:00}.log", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second)

            Using writer As IO.StreamWriter = New IO.StreamWriter(fileName)
                writer.WriteLine("**** エラー ログ: {0} ****", DateTime.Now.ToString())
                writer.WriteLine("このファイルの内容を kiri.feather@gmail.com まで送っていただけると助かります。")
                writer.WriteLine("ご面倒なら@kiri_featherまでお知らせ頂くだけでも助かります。")
                writer.WriteLine()
                writer.WriteLine("動作環境:")
                writer.WriteLine("   オペレーティング システム: {0}", Environment.OSVersion.VersionString)
                writer.WriteLine("   共通言語ランタイム       : {0}", Environment.Version.ToString())
                writer.WriteLine("例外 {0}: {1}", e.Exception.GetType().FullName, e.Exception.Message)
                writer.WriteLine(e.Exception.StackTrace)
                writer.WriteLine()
            End Using

            If MessageBox.Show(String.Format( _
                "エラーが発生しました。ごめんなさい。ログをexeファイルのある場所に {0} として作ったので、kiri.feather@gmail.comまで送っていただけると助かります。{1}ご面倒なら@kiri_featherまでお知らせ頂くだけでも助かります。{1}「OK」ボタンをクリックするとログを開きます。ログを開かない場合は「キャンセル」ボタンをクリックしてください。", _
                fileName, Environment.NewLine), "エラー発生", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) = DialogResult.OK _
            Then
                Diagnostics.Process.Start(fileName)
            End If

            e.ExitApplication = False
        End Sub
    End Class

End Namespace

