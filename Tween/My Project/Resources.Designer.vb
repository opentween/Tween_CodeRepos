﻿'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:2.0.50727.3053
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'このクラスは StronglyTypedResourceBuilder クラスが ResGen
    'または Visual Studio のようなツールを使用して自動生成されました。
    'メンバを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    'ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    '''<summary>
    '''  ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Tween.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        '''  現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        Friend ReadOnly Property At() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("At", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property AtBlink() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("AtBlink", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property AtRed() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("AtRed", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property AtSmoke() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("AtSmoke", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  更新履歴
        '''
        '''***Ver0.1.5.1(Unrelease)
        '''-詳細発言表示部分からの右クリック検索をローカライズ対応可能にした。その他メニュー項目の見直しを行った。
        '''-初期化完了してからフォームを表示するようにした（つもり）
        '''-詳細発言表示部分からの右クリックメニューでURLをコピーできるようにした
        '''-詳細発言表示部分からの検索の際に文字列を正規化していなかったのを修正
        '''-文字列を選択していなかったときに検索をしようとした場合の動作を修正
        '''***Ver0.1.5.0(2008/11/25)
        '''-アイコンサイズ48*48の1列表示が正常に表示されないバグ修正
        '''***Ver0.1.4.0(2008/11/25)
        '''-DMが取れないケースに対応
        '''-DMで、プロテクト状態取得、スクリーンネーム取得に対応
        '''-UI を国際化 (他言語にも容易に対応させることが出来ます)
        '''-UI を英語にローカライズ (仮)
        '''-budurl.comで圧縮されたURLの展開表示に対応
        '''-DMが取得できなかった場合にFollowersリスト取得が実行されなかったのを修正
        '''-詳細発言表示部分で右クリックした場合に [残りの文字列は切り詰められました]&quot;; に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property ChangeLog() As String
            Get
                Return ResourceManager.GetString("ChangeLog", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property MIcon() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("MIcon", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property Refresh() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("Refresh", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property Refresh2() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("Refresh2", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property Refresh3() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("Refresh3", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property Refresh4() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("Refresh4", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property ReplyBlink() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("ReplyBlink", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        Friend ReadOnly Property Repry() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("Repry", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
        
        '''<summary>
        '''  http://en.wikipedia.org/wiki/{0} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property SearchItem1Url() As String
            Get
                Return ResourceManager.GetString("SearchItem1Url", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  http://www.google.com/search?q={0} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property SearchItem2Url() As String
            Get
                Return ResourceManager.GetString("SearchItem2Url", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  http://pcod.no-ip.org/yats/search?query={0} に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property SearchItem3Url() As String
            Get
                Return ResourceManager.GetString("SearchItem3Url", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  http://search.twitter.com/search?q=&amp;amp;ands={0}&amp;amp;phrase=&amp;amp;ors=&amp;amp;nots=&amp;amp;tag=&amp;amp;lang=en&amp;amp;from=&amp;amp;to=&amp;amp;ref=&amp;amp;near=&amp;amp;within=15&amp;amp;units=mi&amp;amp;since=&amp;amp;until=&amp;amp;rpp=15 に類似しているローカライズされた文字列を検索します。
        '''</summary>
        Friend ReadOnly Property SearchItem4Url() As String
            Get
                Return ResourceManager.GetString("SearchItem4Url", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property TabIcon() As System.Drawing.Icon
            Get
                Dim obj As Object = ResourceManager.GetObject("TabIcon", resourceCulture)
                Return CType(obj,System.Drawing.Icon)
            End Get
        End Property
    End Module
End Namespace
