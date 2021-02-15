using System;
using System.Collections.Generic;
using System.Text;

namespace Ikst.KeyboardHook
{
    /// <summary>
    /// キーフックイベントパラメータ
    /// </summary>
    public class KeyboardHookEventArgs : EventArgs
    {
        /// <summary>入力されたキー</summary>
        public VirtualKeys Key { get; internal set; }

        /// <summary>押されている(KeyDown中)キーリスト</summary>
        public List<VirtualKeys> PushedKeyList { get; internal set; }
    }
}
