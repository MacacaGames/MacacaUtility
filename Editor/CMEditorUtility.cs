using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System;

namespace MacacaGames
{
    public class CMEditorUtility
    {

        public static Texture2D CreatePixelTexture(string name, Color color)
        {
            Texture2D texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            texture2D.name = name;
            texture2D.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
            texture2D.filterMode = FilterMode.Point;
            texture2D.SetPixel(0, 0, color);
            texture2D.Apply();
            return texture2D;
        }
        private static Dictionary<string, string> EditorImg = new Dictionary<string, string>()
        {
            {"ImageNotFound","iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAACWElEQVQoFS1SS2/TQBCe2V07TuqkTtPQViiFtEKqQOVAJSROXLlw4ILED+Q39ITEHYESEAJUlT4k1AbapLHj+LGPYbaw9sXa7/P3mEFra3IkpQAEAn8cCAQhbr+InLUghQT0l4iogJxUcjqdp9nCGEIprXNCMIeA7NpaknQ66NHIBCISQgoinM3SPNfGSF2Dfw3WGi8mV6PROF3MHFTOOUZ7BeYSk4VIuknYiJH1A7TW8J+KonRReHF5gWKz3Vr3dhlIJBnkyC3L5bLIUJqoCZbqfFlWpWOiVNHHT6PZbGaMuSUAXwMThLCd1cZKWypF7XbEaZuteFnoyeUNUDAej7MsY1dKkH8sGKFaeS2yqgJpK7Cp0531/saduyrQeTE5/vZDSukzsH9kAeD61DwFDidsYFVoJRW1IY1C2tIJxbrkm1UkPFpSADoAFlQitFyUw5D7ptoadLrX7+1sJo1Gw1viCNyTs4gCndS5S6+qL3/KXxAk/WC43dzuxWG3IyLh0f8sCT8QBxCZOV0dXY/GN4d5MMGyNaC9jQevktX9fJEfnX0fDodxHPMGSCTlwBUwP56PPt+8y4KJFmUgy3ubyc5gi7v8nWbn5+daa5/B8TrwpGR1ubj4OR1N6VSSbi7jJxvPXzx+XS/t+6+HjRYP9P9RKNEC9zA/ORlRkfZpywVwMHz28tGb8ro4/PD2LDvtRrv3acAF3iqQVSAH8RB7iqyPZQTt7z7siOaing2Tg73uU2VpJVmNoogJfm14l7haYxyiJOS2KwVhCOiErklyyABKwAavMBP+AlRiZzNZlw+zAAAAAElFTkSuQmCC"},
            {"PlusButton","iVBORw0KGgoAAAANSUhEUgAAAB4AAAAQCAIAAACOWFiFAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAHBhaW50Lm5ldCA0LjAuNWWFMmUAAABESURBVDhPYxQSEmKgDWCC0jQAo0ajARKMPgUGUA4RYGgGCOF0jSsQzMzMoCwcYEBdDQcQ5xN0LBwM12gkGwzFAGFgAABuGwuxlT4SZwAAAABJRU5ErkJggg=="},
            {"BorderBackgroundGray","iVBORw0KGgoAAAANSUhEUgAAAAUAAAAECAYAAABGM/VAAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAZdEVYdFNvZnR3YXJlAEFkb2JlIEltYWdlUmVhZHlxyWU8AAAAMElEQVQYV2P4//8/Q1FR0X8YBvHBAp8+ffp/+fJlMA3igwUfPnwIFgDRYEFM7f8ZAG1EOYL9INrfAAAAAElFTkSuQmCC"},
            {"ItemNotSelect","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"},
            {"EditPencil","iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAQAAAC1+jfqAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAAmJLR0QAAKqNIzIAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAAHdElNRQfjCAcQCBLW5ejEAAAAn0lEQVQoz6XPMWoCYRCG4YeNLsZGcxjvENLmSEIK0SPYWZguRAi7raRxT5EDpLAUFMfCZXGzv1VmquH9vheG9Ix9+fF8hxqqhHDwksIjAwshhN+UvFI0kWW3vRNC6dGbd71uO+otDGRkrXZp0lx7J+eU/Lrrv/L/4uoGrzy0cV/u8z7m1UzuQ4ju3zAXdSSJ+RbCVN6VQ8+Tja2to0gFLuSUTEAjUjawAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDE5LTA4LTA3VDE0OjA4OjE4KzAyOjAwO2e1mQAAACV0RVh0ZGF0ZTptb2RpZnkAMjAxOS0wOC0wN1QxNDowODoxOCswMjowMEo6DSUAAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAAElFTkSuQmCC"},
            
            {"AlignUtility.H_A","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADMUlEQVRYCe1XS08TURSeV1+hLaWPobTEBwZD2CjRX2BMZCGRxEhYyE8g/gA3Lo2JceFGjQtMcIuJSIJFiCEEFojRRBdsCBo1gn3QjrXtdGbqd2yoUzqXjpTEDTe9uefec+/5vjnn3Ef5SqXC/c8i/E9wwrZFIJfLtc/PL4yvrq5eNAyDyblUKvHLyyun19be9um6bsu2xLRmUiiKcmppaelOLBZbHBgYWBQEQTWpa+Lm5qdgIpF45HK53PF47Go0Gv1aUzIEWwQAyKNS8aIyk0bXNTdwZHgpoOuGj4FZN8wkoGmasLOzI3McL6XT6RhW8eVy2bW9vX1CFEXV4XAmA4H2fJ01jidyVA2e/9PWqy16TAKZTKZzcvLpY8S1F1/kQkwlgPdPTDyZoTzo6Tl5d2Rk5CE8YmHW/hCTALanu1gs9qH28PgcKgD2oN8LMlyppB63D8Oe2Yw+XFkFN5ugMfwatgNSpZYfmFOTzWv3ykwP7J24Xz+VSvlVVW1DG8U8Cd4TEa5OyDmPx1MIBAJZ1vqWCSAswuzsy1sbGxsXAEIePYYEFqenXzyAXJblSGJ0dPSm3+8vW5FomQCBYvudxQ45Y05IkOinYz6ZTH0uFAoSi0CzHLAibTXWkA80qZorvGUe7Ro5LAK79v65PSJw5IEjDxyWByzt0EFExzK1rHIYJ6EhitKCw+HQAeLATXmOWkmS3qAthMOhFdwHlscwkWqZAI5fY3Dw0m1cRvdwH8Tm5l49B3j70NDlcVmW1wGu4RjWCMyqtEyAjIZCIQLQstksvZDo6DUAnu/q6vpF+v2KZexMCyzjV40t1/BQMIy/scacBr3Jbk1kegBfobjd7tdov+HKbYOL6bZTnE7nB/Q1l8v5rmalBYFJoKOjIzk2dv0G7jRxa+v7+ampZzORSGR9ePjKNTxKi3iUKubr96AcmASQSFw4HFbIsKqWMnCpgLEiYvsDwIysrvA0r1rtUWISMC/3er1f4vH4/e7u+HuEhJnRwWDwJwh+RLgkURTSZhssmRizdHXj+XxeQPwN7Pe68b0d/IsK4TXE+Xy+FHmxWbFNoJmhg+qbbcOD2rW97je8Pz7sgF/2NAAAAABJRU5ErkJggg==" },
            {"AlignUtility.H_C","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACmklEQVRYCe1WvU7rMBi9ToGi/i23AgEba7cwIFFGVgbERJgRMPMKrOwg9jAhBl4BxEQegY1bXQQSpT9SQcScE+LiVk5rhYouRKryxfZ3zrH9+dRCSvlnnI8zTnJyT6QV0Ol0MkEQLCPfcV33JpvNvqfB+o6A8tXV9QlJK5XKGgT8/1EBIJt0HOdvTDqZhpw5qVdACBEiP6rgOE6lwUpAGIZOu90u4z1FFhI2Go0FKcMMvxnzjRMVFTVW5jWXyz3iTZEDH2FzDJvN5ozvn52DaFHNFmIyrVZrhuj5fP4BZFERUkSxWLzzvK3NQqHwMJAdnbYrMEXyer0+DwFdTBWjb041qgmp1VLtSW8rAUzmzEmoSHXA/jaO1fsHxWM3ol8BtjUgUVQCv54aQOVH28t29bAIORbfVv9yVgJgs8/V6soh/B/VLggsEZeC4HaHxK67dIoxLwhBLAXiGnPYN+yx8gETCDxg9vj4JGDf3t6uCy8Y3X8BlxTOxyU3FimPGc7+NJww0gZ/mOay04RMYumIcEaptkwfY1wBOF8JzndAgwGZaS9ZEwWswgbBMPsLgDcRfrlUzAJRAs5YgzMewRm5TT2PsQYAXgL5DmZGAT0J+ofqw9htvV2PY2esAfMU7XYCCADwdxIoEh20Px42hlj9OerbuGeq8yfevwKMRcil55GKCyhxJ9TeDxrHPmIlgRgF4Ei98lLBJJB8+ayGgqruXkh48UCOsdBITixiaund0OgDAO+5gnVHxwFF4egt+L5/ySbP89ZBcp80U5InXdGSViC0uU4J8TlrkuP3r1+ozbdRgE1iPNvIpZJmboOTWgDA37BVTzHJmw2ZaUxqAfi7fVxdre4DNGRsArdpMxahTeKoxiSez1ERDMP5AMRTK9/mtxu5AAAAAElFTkSuQmCC" },
            {"AlignUtility.H_L","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACK0lEQVRYCd2Wv0/CQBTHubYSkF9qmhiJjLIR4x9AJJI4OcrkIgsm/j1uapzVAXcNm5OJi4MJDCYMEENlAFMQ2uJ7YhMCd/VZkCZeQkrv3r33uXvfez02GAx8XjbJy+AYmwtQqVQSpVJpr16vr/w1IBegXC4f3d7eXddqtV1PABiTlmVZDjDGIp4AQFBbmfbzzzgUimfTNCVd11U4MX5ne9YJhRabsHtkcBKApmnq5eXVhWEYWwBgCiAgrlzK5fYL8XhcF9hMdJMADMNUWq1WvN/vr014GOlQFCUBu7Uw0vXjXxIAYz4fCNKCn6NDtAED8vajM+4xdIwy48F/CeCcp7EdJGkAP1iWZSkgMNTCmIvhK9rAGIMn6oDcSADRaLSVyWyfAMA6yFEgsgGTJOk5FlsiH0GkJAFEIpH3dDp9Sl7WLwxJAOiv2+2yXq/nSrSYmXA4zD3GJIB2ux0qFm+OG43GBjr7xQK/TCE13Wx25yyVSj2NzyUBQBWMVavVA6iEm+MOKO+gHQvmP7oGgFWjwk18umnDufzs8XvdRHGeI0zbvACEeJ4DkESIN3eocBJWOzftey5XQCQARZE/oBq+wIVkFQBEFxIRG4MyrgeDwVeeAQlAVdVmPn9YgJUEeE4IfZbf73/j2ZEA8I4H5VjjOZi2TyRCO1/2c9o4wvlcAPiialC9OrDlbeHMGQ1wU5BMJs9BNA9wu72fURyhG7xACAfnMcBNwTwC2zE+ASMiuMsHZYnlAAAAAElFTkSuQmCC" },
            {"AlignUtility.H_R","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACMUlEQVRYCdWXTUsCQRiAnd1VXMGPJJEOdvPYpbzkQbRLlw55yXPUsT/TJfoDQWT/wFzoF4QgCO5BsFA2pMD8jN21d/IDd51dZzeXJWHYmXffj+edeWdmRZPJxOPmj3EzOI7tOEC73d4SBOFEFMVdUrKOA7RareNS6emhXq9fuAKAEAqyLMsjxGy7AgBB51U+f2o4OM1ozUBRFNTvD6LgkzdThay/A4FABzJXzfTwO0sAkiTxxeLjNYAcga1i4JzlOO6lUDg7j8fj7wY6C7ElAAjs7Xa7CVmWdxYeCB2v1yvJskLlm0ppKcYEpleFtiRa7U51VuUkiePbkBR0WfbvAMznfjk1yr6lGoCLS8VbEZrHqA7w5Qbbj6O95CwBhMORQS6XvVVV9dnjQcSDBc4IBABvoVCoSzMJFgFCSiaTuadxTKtDBMDT1+v1GHjaWnOfz6f6/X6DGdKiEQGq1epeuSxcwlT7terrRxg6FouJ+fzpTTAY7K+zIAI0m839TqdzBWtpa5sOh8MKnJh3tgHg6sRVrkCzBTCzXZf873uzAFRrSBXFRMkMwMRsc69cByAWIa5kaHgb2kp1aktnSgTgeV6KRCINhmEC4MYqBf4gaXAcO6ZBIAKk04dCKnWQBQe2lgh2wQg+yT5sA8ApNobWonHwVx1bGVoMOj/O50+NueMAUJBfcH2P4Cb/1ESeDXC1k+Qbk8Ffs2itVksnEolKMpl81Tt2HEAfUD92fAn0AfXjH1fwuKOdn9NEAAAAAElFTkSuQmCC" },
            {"AlignUtility.V_A","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAADMklEQVRYCe1WS2tTQRTOnZubB0nTvMhDbIWKtgrSfyAYROrG7rISLIi/QLryL7hw4Vaw26xaN5aAkboRTFviUoVaQRKa98OQ3tyX3yS5Q26axzWVdpOB6e2cc+acb76Z+SacpmmWy2zkMovT2jMAMwasZg9hvV4XWq2WleO4sVPorXI6nZLH45HHBvacpgBUq9X5RCLxolgsrQCAOi4xAPDBYOBzPB5/6fV62+Niqc8UAKzcmc8XHkiStGqGAcQ6MOfVPwFQFIUUCoWooqhzYJmpEwpq+Xw+DLDCpOJ0Rb0YAM4vY9gEI2zPqObxPCkHg8GC1dpdO6cr4cHB4UoymXyrquoiJhpoprTKshyAXUA300QUKAPMYB7icrm+bGw8eerz+Uo0EdsCURRV9FPYquiMARqERuO86GYBSABcQ/wZADabTdZXD7+FMYDi3PHxL7+iyA5qpk7aCOG0UqkUSaU+bmGb7nSt4//yPL8Xi917FggEmqrKUmGSxvn9/j+RSISC6zTGgN1u15aXb3Zo0Z36N5fLWQAAu2NYkO4+8wUAaWlpKRuNRptnnAMGBmDAbhiCtiYSpsBOGY5JKAjPW/cwRzQkGTFgWzDCz8xYPZVts9KtEkImAe3kHgnALN0M4YR/AGhoxNAtqFZrrt3d949FsX0NmmBqJUOzw4i7z9nttsza2sNtr3deGowbCkCS2sGjo5/PoWY3zIjPYNL+MdUZvA2fkPMD7PQMGRoDQClHt4EqDdeNfqFavCF42gFyWZHTgfxUTWV0djfZGUin0/czma+bCHZD8+2Q0tuY4Jy2aP885KyHQqFvEKDThYWrb2Kx2JYgdDWNMVCvN25ls9m7CKanhUNx5utPNs3/yDWHd2YV20Gw+v12u00AoHO2GAN47+cbjcZ11OfK5fKVnZ13rxG4OE3BwTnQhMz6+qNN6H/F7Xb/xm+FEz2GrRLGGvohdQiC7QRARHqA/schRK5mOBzZx+8E+s4YGgNgsFo0GdL8AwDoKTzXNcR8Hrm+40IqxhrdEduCfideMlKpVMIAgIfp/A0sNkB/sf8V1LMOBaA7L+I7XB8vonKvxgzAjIG/9utfl+qfpFYAAAAASUVORK5CYII=" },
            {"AlignUtility.V_B","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACC0lEQVRYCe1WzU7CQBCmtPxIbBMRQiCBwNGEJ1ASEyO+gd65eTM+iAcT776EF4MJntATETxwwJumvdTYQkwJ9sdvTUq2kKXIT3phLzudnZ3vm9mZ3XKO44SCHOEgwQn2hkDgGRBYNTAcDmPN5tORYRgZjuM8leo4dqhQKLTK5fIr1lgu5tIzCYxGo91Wq3WtaVoJIDbljUPn8L3e202xWLwURZFeo8zmE5kEsD2MkeB5XpiMkrSubdtxzMuFT0B8eHpS72O70LIfgYWc/mdT4ARm1cDcgeh6n9d1LYFaIQHRx8ahhn4ymYyBmdaPfa+EQLv9ctpoPJ5PgqBIw5IkvddqtQtJEj/HqJSwEgLoiD3TNA9Jd9CDfFuWpSApW7SelldUA5wz2aoUiEXJU+KKCEz5nVuxIbDJgF8GeFY1ocWoh8ghLyTLlOmDbGDeA2iroSAIz5FIpATZ8+QCjE+n071oNPqnxwX0AbmD19OkWcAuDB8y9o9oPS0zmeMC4VRVTZqmFZv85yDR4obT8S/wTZwNBoPtfr8vAYj2HSJJEQTeTKVSKkh6gnANmQRcg3XP4yNQFCUpy/IJohAByjzQJQmRgAe5XO4+m81+EV9jAt1ud79ef7hFquJLgszcjqM1qtXjMxC48xDI5/PtSuXgCi/qDhbWmAFbBVbHZRl4DfjdAy7Rtc0bAoFn4Be58bL5hpG7RAAAAABJRU5ErkJggg==" },
            {"AlignUtility.V_C","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAChUlEQVRYCe1WMW/aQBjFdlskjEulsNBIHbKysaHyHzp0aqTO2TNkSYf8iwwdq6RThqp/AYeNn5ChUopaUalAsWJHsfteyjl3h88BYcqCpdP57t597/l99x1YSZKUNvnYmyQn91bAxh14YjoDcRzbQRDU0T/Lwti2Pa5UKmP0WcsLzxkFkPz8/PPFZDLZsywrliOichzP8z7u7787qVarK5WRUQC/nOSj0eglBMj8pVnpNugSFu6UxSUHRgGMwy8nuS5gtrbSlwuduQIE6LE+DEMX7QVwqlWYwBmJcFaG6JU0ipiFCOj3++99//IDSBRXkCobZ+UKZ+UtzspPQSr3hQjA1zdwVnb1ihDXvKmSKKQQAXA+yTkrmdYLF4xFzAMoQIaepAKjWK/jJZy+9OAAbHTQ6kA85QaU4C6sc+Z2zCawVgXmFWy/wb7nOTiHsbjOM4HutlwuD9Huy9cSeer1ep1u1z9FwJ1/2NiZToM61jNFQGTgupXflmWXoij0wjDySKI/wN0BNwSOhBaE/+p0Xh+02+1LYuUUCDv1GOsYp7ypA1kpwFX8BfY18BWKCLqG8jpDeR3XarUbOHfk+/5hVhUANwDuDfrrrBSkVcCcoP2QmRDQeM1i7Q+CfnNdN8G+sbxPfmcMkqN9l+fFe2qFmBD9TK0YZvV0T+xXLdLQEk5bKeweSO5TCSKFgOM8coLTFCg7lxwgBQOchWvYrSggOay/wnxkClmIgFar9anZbH4FyVwqSM4fo7UKgANTNhNJ3nyuA7SQedQfzqHNfa2OW2RsFEDrmD8G4dUsBwM5/5INgFHmZcyi7+lFpG/Alflf/pQaBeiC1jUWF8m64j8adytg4w78BRhEIECgQSfGAAAAAElFTkSuQmCC" },
            {"AlignUtility.V_T","iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAACC0lEQVRYCe1WQUsCQRR2dhfT0l0oQZIQOyh4sfDUH0nolnUNOvUv+gN171CduxdURIfwFuhxoUMakUsquuv0TbQ2s7FjqIseXFhmZt+b97737Zv3hlBKQ9N8lGk6Z77nAKbOgObmQK1WS1er1X1ClAS+BZWZhNL+ey6XO8lmsybzPQBgmmbh9vbuSFXVqAsqiNFxnE40Gn34AyCfz98ZhrFHCInDcYAMUCuVSt27wYGSoHy5LuTj4Bd41UAVaTQay7btLBAiShloXdc/4vH4J5NYlhVrNps62BMUWWyaptqJRKKBX9sXhD8LXwCtVmvl/PziFIbXYVjYDABqOp0+K5W2jyORCK1UKjvX1zcHiqLYvBPoKQD6Ui7vlgH2lZe5c18A2ByxbXur1+ut/o2Mhur1+mO321UAAGQ5a5gXEKVr93tkTMFGEmNYEHCLYXXA4XSFKUBxyUOoFySn7GuD6QwDwNkJZjoHMGdgQgxQWUUVz6Ynl33rgEdPukQBetY07QZ1gDuaaCgoRPiGrkfafgYmAmBjY/Myk8lcoRYwRnkQBAB6S0uLwQIwDN3Ba/lFKfs+oRyQuZDLZh6A2F/lwYwklSVhH08Lnc5GcvHtmB05FZnfERvSSP5/74Te7eFw+K1YLB622+2k1xEuliHcB55isRgPzGviX2tZAfmXgXGVZj4Jxw1w6P45A18CYbTCP3mzxgAAAABJRU5ErkJggg==" },
        };

        public static Texture2D TryGetEditorTexture(string name)
        {
            Texture2D result;
            if (s_Cached == null)
            {
                LoadResourceAssets();
            }
            if (!s_Cached.TryGetValue(name, out result))
            {
                result = CreatePixelTexture(name, Color.gray);
                s_Cached.Add(name, result);
            }

            return result;
        }
        private static Dictionary<string, Texture2D> s_Cached;
        /// <summary>
        /// Read textures from base-64 encoded strings. Automatically selects assets based upon
        /// whether the light or dark (pro) skin is active.
        /// </summary>
        public static Dictionary<string, Texture2D> LoadResourceAssets()
        {
            if (s_Cached != null)
            {
                return s_Cached;
            }
            s_Cached = new Dictionary<string, Texture2D>();
            //for (int i = 0; i < s_Cached.Length; i++)
            foreach (var item in EditorImg)
            {
                byte[] array2 = System.Convert.FromBase64String(item.Value);
                int width = 1;
                int height = 1;
                GetImageSize(array2, out width, out height);
                Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
                texture2D.hideFlags = (HideFlags.HideInHierarchy | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset);
                texture2D.name = "(Generated) ReorderableList:" + item.Key;
                texture2D.filterMode = FilterMode.Point;
                texture2D.LoadImage(array2);
                s_Cached.Add(item.Key, texture2D);
            }
            return s_Cached;
        }
        private static void GetImageSize(byte[] imageData, out int width, out int height)
        {
            width = ReadInt(imageData, 18);
            height = ReadInt(imageData, 22);
        }
        private static int ReadInt(byte[] imageData, int offset)
        {
            return (int)imageData[offset] << 8 | (int)imageData[offset + 1];
        }

        public static void InspectTarget(UnityEngine.Object target)
        {
            ViewInInspectorInstance(target);
        }
        public static UnityEditor.EditorWindow ViewInInspectorInstance(UnityEngine.Object viewTarget, UnityEditor.EditorWindow inspectorInstance = null)
        {
            var inspectorType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            if (inspectorInstance == null)
                inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as UnityEditor.EditorWindow;

            if (inspectorInstance.GetType() != inspectorType)
                throw new System.NotImplementedException();

            inspectorInstance.Show();

            System.Reflection.BindingFlags bindingFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            System.Reflection.MethodInfo isLockedMethodInfo = inspectorType.GetProperty("isLocked", bindingFlags).GetSetMethod();
            isLockedMethodInfo.Invoke(inspectorInstance, new object[] { false });       //解除InspectorLock
            var prevSelection = UnityEditor.Selection.objects;                          //記錄前一個選擇的物件
            UnityEditor.Selection.objects = new UnityEngine.Object[] { viewTarget };    //選擇viewTarget讓Inspector刷新
            isLockedMethodInfo.Invoke(inspectorInstance, new object[] { true });        //Inspector Lock
            UnityEditor.Selection.objects = prevSelection;                              //重新選擇前一個物件，當作什麼都沒發生

            return inspectorInstance;
        }

    }
    public class CustomElement
    {
        static Dictionary<int, bool> control = new Dictionary<int, bool>();
        static void SetFocus(int id, bool target)
        {
            if (control.ContainsKey(id))
            {
                control[id] = target;
            }
            else
            {
                control.Add(id, target);
            }
        }
        static bool GetFocus(int id)
        {
            if (control.TryGetValue(id, out bool focus))
            {
                return focus;
            }
            else
            {
                return false;
            }
        }
        public static bool Button(int id, Rect rect, GUIContent content, GUIStyle style, bool processEvent)
        {
            bool result = false;


            var e = Event.current;
            switch (e.type)
            {
                case EventType.Repaint:
                    var isMouseOver = rect.Contains(Event.current.mousePosition);
                    style.Draw(rect, content, isMouseOver, GetFocus(id), GetFocus(id), GetFocus(id));
                    break;
                case EventType.MouseDown:
                    if (processEvent == false)
                    {
                        SetFocus(id, false);
                        GUI.changed = true;
                        return result;
                    }
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            SetFocus(id, true);
                            e.Use();
                            GUI.changed = true;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (processEvent == false)
                    {
                        SetFocus(id, false);
                        GUI.changed = true;
                        return result;
                    }
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            SetFocus(id, false);
                            result = true;
                            GUI.changed = true;
                        }
                    }
                    break;

            }
            return result;
        }
    }
    public class CMEditorLayout
    {
        #region  BitMask
        static GUIStyle _toggleStyle;
        static GUIStyle toggleStyle
        {
            get
            {
                if (_toggleStyle == null)
                {
                    _toggleStyle = new GUIStyle
                    {
                        normal = {
                                background = CMEditorUtility.CreatePixelTexture("_toggleStyle_on",new Color32(64,64,64,255)),
                                textColor = Color.gray
                            },
                        onNormal = {
                                    background = CMEditorUtility.CreatePixelTexture("_toggleStyle",new Color32(128,128,128,255)),

                                 textColor = Color.white
                            },

                        alignment = TextAnchor.MiddleCenter,
                        clipping = TextClipping.Clip,
                        imagePosition = ImagePosition.TextOnly,
                        stretchHeight = true,
                        stretchWidth = true,
                        padding = new RectOffset(0, 0, 0, 0),
                        margin = new RectOffset(0, 0, 0, 0)
                    };
                }
                return _toggleStyle;
            }
        }
        public static void BitMaskField<T>(ref T enumValue) where T : System.Enum
        {
            Dictionary<int, bool> toggleBools = new Dictionary<int, bool>();
            int possiableInt = System.Enum.GetValues(typeof(T)).Cast<int>().Max();
            foreach (T item in System.Enum.GetValues(typeof(T)))
            {
                int intValue = System.Convert.ToInt32(item);
                if (intValue == 0 || intValue == possiableInt)
                {
                    toggleBools.Add(intValue, object.Equals(enumValue, item));
                    continue;
                }
                toggleBools.Add(intValue, FlagsHelper.IsSet(enumValue, item));
            }
            using (var horizon = new EditorGUILayout.HorizontalScope())
            {
                foreach (T item in System.Enum.GetValues(typeof(T)))
                {
                    int intValue = System.Convert.ToInt32(item);

                    using (var check = new EditorGUI.ChangeCheckScope())
                    {
                        toggleBools[intValue] = GUILayout.Toggle(toggleBools[intValue], item.ToString(), toggleStyle);
                        if (check.changed)
                        {
                            if (intValue == 0 || intValue == possiableInt)
                            {
                                if (toggleBools[intValue])
                                {
                                    enumValue = item;
                                }
                                continue;
                            }
                            if (toggleBools[intValue])
                            {
                                FlagsHelper.Set(ref enumValue, item);
                            }
                            else
                            {
                                FlagsHelper.Unset(ref enumValue, item);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region  GroupedPopup
        static Dictionary<int, Rect> rectGroupedPopupFieldDict = new Dictionary<int, Rect>();
        public static void GroupedPopupField(int id, GUIContent content, IEnumerable<GroupedPopupData> groupedPopupData, GroupedPopupData selected, System.Action<GroupedPopupData> OnSelect, params GUILayoutOption[] layoutOptions)
        {
            if (!rectGroupedPopupFieldDict.ContainsKey(id))
            {
                rectGroupedPopupFieldDict.Add(id, new Rect());
            }

            if (content != GUIContent.none) EditorGUILayout.LabelField(content, GUILayout.Width(EditorGUIUtility.labelWidth));
            string popupTitle = "";

            if (groupedPopupData.Contains(selected))
            {
                popupTitle = selected.name;
            }
            else if (selected == null)
            {
                popupTitle = "Nothing Selected";
            }
            else
            {
                popupTitle = "Missing";
            }
            if (GUILayout.Button(popupTitle, EditorStyles.popup, layoutOptions))
            {
                PopupWindow.Show(rectGroupedPopupFieldDict[id], new GroupedPopupWindow { groupedPopupData = groupedPopupData.ToArray(), Current = selected, OnSelect = OnSelect, WantedWidth = rectGroupedPopupFieldDict[id].width });
            }
            if (Event.current.type == EventType.Repaint) rectGroupedPopupFieldDict[id] = GUILayoutUtility.GetLastRect();
        }

        public static void GroupedPopupField<T>(int id, GUIContent content, IEnumerable<GroupedPopupData<T>> groupedPopupData, GroupedPopupData<T> selected, System.Action<GroupedPopupData<T>> OnSelect, params GUILayoutOption[] layoutOptions) where T : struct
        {
            if (!rectGroupedPopupFieldDict.ContainsKey(id))
            {
                rectGroupedPopupFieldDict.Add(id, new Rect());
            }

            if (content != GUIContent.none) EditorGUILayout.LabelField(content, GUILayout.Width(EditorGUIUtility.labelWidth));
            string popupTitle = "";

            if (groupedPopupData.Contains(selected))
            {
                popupTitle = selected.item.ToString();
            }
            else if (selected == null)
            {
                popupTitle = "Nothing Selected";
            }
            else
            {
                popupTitle = "Missing";
            }
            if (GUILayout.Button(popupTitle, EditorStyles.popup, layoutOptions))
            {
                PopupWindow.Show(rectGroupedPopupFieldDict[id], new GroupedPopupWindow<T> { groupedPopupDataGeneric = groupedPopupData.ToArray(), CurrentGeneric = selected, OnSelectGeneric = OnSelect, WantedWidth = rectGroupedPopupFieldDict[id].width });
            }
            if (Event.current.type == EventType.Repaint) rectGroupedPopupFieldDict[id] = GUILayoutUtility.GetLastRect();
        }

        public class GroupedPopupWindow<T> : GroupedPopupWindow where T : struct
        {
            public GroupedPopupData<T>[] groupedPopupDataGeneric;
            public System.Action<GroupedPopupData<T>> OnSelectGeneric;
            public GroupedPopupData<T> CurrentGeneric;

            protected override void DrawItem()
            {
                using (var scroll = new GUILayout.ScrollViewScope(scrollPos))
                {
                    scrollPos = scroll.scrollPosition;
                    using (var vertical = new GUILayout.VerticalScope())
                    {
                        var grouped = groupedPopupDataGeneric.GroupBy(m => m.group);

                        foreach (var item in grouped)
                        {
                            if (!string.IsNullOrEmpty(searchString))
                            {
                                if (searchString.ToLower().Contains("g:"))
                                {
                                    var s = searchString.ToLower().Split(':');
                                    if (!item.Key.ToLower().Contains(s.Last().ToLower()))
                                    {
                                        continue;
                                    }
                                }
                            }
                            string label = string.IsNullOrEmpty(item.Key) ? " Ungrouped" : " " + item.Key;
                            GUILayout.Label(label, GroupHeader);
                            foreach (var child in item)
                            {
                                if (!string.IsNullOrEmpty(searchString))
                                {
                                    if (!searchString.ToLower().Contains("g:"))
                                    {
                                        if (!child.item.ToString().ToLower().Contains(searchString.ToLower()))
                                        {
                                            continue;
                                        }
                                    }
                                }
                                var contetn = new GUIContent(child.item.ToString());
                                if (CurrentGeneric != null)
                                {
                                    if (CurrentGeneric.item.Equals(child.item))
                                    {
                                        contetn.image = EditorGUIUtility.FindTexture("d_P4_CheckOutRemote");
                                    }
                                }
                                if (GUILayout.Button(contetn, ItemStyle))
                                {
                                    OnSelectGeneric?.Invoke(child);
                                    editorWindow.Close();
                                }
                                Rect btnRect = GUILayoutUtility.GetLastRect();
                                if (btnRect.Contains(Event.current.mousePosition))
                                {
                                    //GUI.Box(btnRect, "", new GUIStyle("U2D.createRect"));
                                    editorWindow.Repaint();
                                }
                            }
                        }
                    }

                }
            }
            // string searchString;
            // protected override void DrawSerachBar()
            // {
            //     GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            //     searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
            //     var rect = GUILayoutUtility.GetLastRect();
            //     if (string.IsNullOrEmpty(searchString))
            //     {
            //         GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            //         rect.x += 15;
            //         rect.width -= 15;
            //         GUI.Label(rect, "g: for find by group", new GUIStyle("AnimationSelectionTextField"));
            //         GUI.color = Color.white;
            //     }
            //     if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            //     {
            //         // Remove focus if cleared
            //         searchString = "";
            //         GUI.FocusControl(null);
            //     }
            //     GUILayout.EndHorizontal();
            // }
        }

        public class GroupedPopupWindow : UnityEditor.PopupWindowContent
        {
            public GroupedPopupData[] groupedPopupData;
            public System.Action<GroupedPopupData> OnSelect;
            public GroupedPopupData Current;
            public float WantedWidth;
            static GUIStyle _ItemStyle;
            protected static GUIStyle ItemStyle
            {
                get
                {
                    if (_ItemStyle == null)
                    {
                        _ItemStyle = new GUIStyle(EditorStyles.label);
                        // _ItemStyle.contentOffset = Vector2.zero;
                        _ItemStyle.hover.textColor = Color.white;
                        ColorUtility.TryParseHtmlString("#49beb7", out Color c);
                        _ItemStyle.hover.background = MacacaGames.CMEditorUtility.CreatePixelTexture("_ItemStyle hover Pixel (List GUI)", c);
                    }
                    return _ItemStyle;
                }
            }
            static GUIStyle _GroupHeader;
            protected static GUIStyle GroupHeader
            {
                get
                {
                    if (_GroupHeader == null)
                    {
                        _GroupHeader = new GUIStyle(EditorStyles.label);
                        _GroupHeader.alignment = TextAnchor.MiddleCenter;
                        _GroupHeader.normal.textColor = Color.white;
                        ColorUtility.TryParseHtmlString("#ff502f", out Color c);
                        _GroupHeader.normal.background = MacacaGames.CMEditorUtility.CreatePixelTexture("_group header Pixel (List GUI)", c);
                    }
                    return _GroupHeader;
                }
            }
            // public GroupedPopupWindow(IEnumerable<GroupedPopupData> groupedPopupData, GroupedPopupData current, System.Action<GroupedPopupData> OnSelect, float wantedWidth = 200)
            // {
            //     this.OnSelect = OnSelect;
            //     this.groupedPopupData = groupedPopupData.ToArray();
            //     this.current = current;
            //     this.wantedWidth = wantedWidth;
            // }
            public override Vector2 GetWindowSize()
            {
                return new Vector2(300, WantedWidth);
            }
            public override void OnGUI(Rect rect)
            {
                DrawSerachBar();
                DrawItem();
            }
            protected Vector2 scrollPos;
            protected virtual void DrawItem()
            {
                using (var scroll = new GUILayout.ScrollViewScope(scrollPos))
                {
                    scrollPos = scroll.scrollPosition;
                    using (var vertical = new GUILayout.VerticalScope())
                    {
                        var grouped = groupedPopupData.GroupBy(m => m.group);

                        foreach (var item in grouped)
                        {
                            if (!string.IsNullOrEmpty(searchString))
                            {
                                if (searchString.ToLower().Contains("g:"))
                                {
                                    var s = searchString.ToLower().Split(':');
                                    if (!item.Key.ToLower().Contains(s.Last().ToLower()))
                                    {
                                        continue;
                                    }
                                }
                            }
                            string label = string.IsNullOrEmpty(item.Key) ? " Ungrouped" : " " + item.Key;
                            GUILayout.Label(label, GroupHeader);
                            foreach (var child in item)
                            {
                                if (!string.IsNullOrEmpty(searchString))
                                {
                                    if (!searchString.ToLower().Contains("g:"))
                                    {
                                        if (!child.name.ToLower().Contains(searchString.ToLower()))
                                        {
                                            continue;
                                        }
                                    }
                                }
                                var contetn = new GUIContent(child.name);
                                if (Current != null)
                                {
                                    if (Current.name == child.name)
                                    {
                                        contetn.image = EditorGUIUtility.FindTexture("d_P4_CheckOutRemote");
                                    }
                                }
                                if (GUILayout.Button(contetn, ItemStyle))
                                {
                                    OnSelect?.Invoke(child);
                                    editorWindow.Close();
                                }
                                Rect btnRect = GUILayoutUtility.GetLastRect();
                                if (btnRect.Contains(Event.current.mousePosition))
                                {
                                    //GUI.Box(btnRect, "", new GUIStyle("U2D.createRect"));
                                    editorWindow.Repaint();
                                }
                            }
                        }
                    }

                }
            }
            protected string searchString;
            protected virtual void DrawSerachBar()
            {
                GUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
                searchString = EditorGUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
                var rect = GUILayoutUtility.GetLastRect();
                if (string.IsNullOrEmpty(searchString))
                {
                    GUI.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
                    rect.x += 15;
                    // rect.width -= 15;
                    GUI.Label(rect, "g: for find by group");
                    GUI.color = Color.white;
                }
                if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
                {
                    // Remove focus if cleared
                    searchString = "";
                    GUI.FocusControl(null);
                }
                GUILayout.EndHorizontal();
            }
        }

        public class GroupedPopupData
        {
            public string name;
            public string group;
        }

        public class GroupedPopupData<T> where T : struct
        {
            public T item;
            public string group;
        }
        #endregion
    }
}
