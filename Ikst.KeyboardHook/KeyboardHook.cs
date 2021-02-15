using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace Ikst.KeyboardHook
{
    /// <summary>
    /// ローレベルグローバルキーフック
    /// </summary>
    public class KeyboardHook
    {

        private const int WM_KEYDOWN = 0x100;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYUP = 0x105;

        private const int WH_KEYBOARD_LL = 13;


        /// <summary>
        /// キーフックプロシージャのデリゲート
        /// </summary>
        /// <param name="nCode">フックコード</param>
        /// <param name="wParam">仮想キーコード</param>
        /// <param name="lParam">キーストロークメッセージ</param>
        /// <returns>ハンドル</returns>
        internal delegate IntPtr KeyboardHookHandler(int nCode, IntPtr wParam, IntPtr lParam);


        /// <summary>
        /// キーボードフックイベントのデリゲート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void KeyboardHookCallback(object sender, KeyboardHookEventArgs e);


        /// <summary>キーを押下した時のイベント</summary>
        public event KeyboardHookCallback KeyDown;

        /// <summary>キーを離した時のイベント</summary>
        public event KeyboardHookCallback KeyUp;


        /// <summary>キーフックのコールバック処理を保持する</summary>
        private KeyboardHookHandler hookHandler;

        /// <summary>フックプロシージャのハンドル</summary>
        private IntPtr hookID = IntPtr.Zero;

        /// <summary>押下されているキーのリスト</summary>
        private List<VirtualKeys> downKeyList = new List<VirtualKeys>();


        /// <summary>開始状態</summary>
        public bool IsStarted { get; private set; }


        /// <summary>
        /// キーフック開始
        /// </summary>
        public void Start()
        {
            if (!IsStarted)
            {
                downKeyList.Clear();

                // キーフックプロシージャ
                hookHandler = HookFunc;

                // キーフックプロシージャをフックチェーン内に登録
                hookID = SetHook(hookHandler);

                IsStarted = true;
            }
        }


        /// <summary>
        /// キーフック終了
        /// </summary>
        public void Stop()
        {
            if (IsStarted)
            {

                downKeyList.Clear();

                // キーフックの解放
                NativeMethods.UnhookWindowsHookEx(hookID);
                hookID = IntPtr.Zero;

                IsStarted = false;
            }
        }


        /// <summary>
        /// キーフックプロシージャ
        /// </summary>
        /// <param name="nCode">フックコード</param>
        /// <param name="wParam">仮想キーコード</param>
        /// <param name="lParam">キーストロークメッセージ</param>
        /// <returns>ハンドル</returns>
        private IntPtr HookFunc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (0 <= nCode)
            {
                int iwParam = wParam.ToInt32();

                // キーダウン
                if ((iwParam == WM_KEYDOWN || iwParam == WM_SYSKEYDOWN))
                {
                    VirtualKeys vk = (VirtualKeys)Marshal.ReadInt32(lParam);
                    if (!downKeyList.Contains(vk))
                    {
                        downKeyList.Add(vk);
                    }
                    if (KeyDown != null)
                    {
                        KeyboardHookEventArgs args = new KeyboardHookEventArgs();
                        args.Key = vk;
                        args.PushedKeyList = downKeyList;
                        KeyDown(this, args);
                    }
                }

                // キーアップ
                if ((iwParam == WM_KEYUP || iwParam == WM_SYSKEYUP))
                {
                    VirtualKeys vk = (VirtualKeys)Marshal.ReadInt32(lParam);
                    if (downKeyList.Contains(vk))
                    {
                        downKeyList.Remove(vk);
                    }
                    if (KeyUp != null)
                    {
                        KeyboardHookEventArgs args = new KeyboardHookEventArgs();
                        args.Key = vk;
                        args.PushedKeyList = downKeyList;

                        KeyUp(this, args);
                    }
                }

            }

            return NativeMethods.CallNextHookEx(hookID, nCode, wParam, lParam);
        }


        /// <summary>
        /// キーフックをフックチェーン内に登録する
        /// </summary>
        /// <param name="proc">フックプロシージャ</param>
        /// <returns>フックプロシージャのハンドル</returns>a
        private IntPtr SetHook(KeyboardHookHandler proc)
        {
            using (ProcessModule module = Process.GetCurrentProcess().MainModule)
            {
                return NativeMethods.SetWindowsHookEx(WH_KEYBOARD_LL, proc, NativeMethods.GetModuleHandle(module.ModuleName), 0);
            }
        }


        /// <summary>
        /// デストラクタ
        /// </summary>
        ~KeyboardHook()
        {
            Stop();
        }

    }
}
