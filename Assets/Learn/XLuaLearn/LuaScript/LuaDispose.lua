local util = require 'xlua.util'
util.print_func_ref_by_csharp()

xlua.hotfix(CS.XLuaLoad, 'Func', nil)
xlua.hotfix(CS.XLuaLoad, 'Play', nil)
xlua.hotfix(CS.XLuaLoad, 'Play1', nil)
xlua.hotfix(CS.XLuaLoad, 'Awake', nil)



