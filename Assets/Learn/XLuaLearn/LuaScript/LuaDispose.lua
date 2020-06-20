local  util = require 'util'

print(util==nil)

util.print_func_ref_by_csharp()

xlua.hotfix(CS.XLuaLoad, 'Func', nil)
xlua.hotfix(CS.XLuaLoad, 'Play', nil)
xlua.hotfix(CS.XLuaLoad, 'Play1', nil)
xlua.hotfix(CS.XLuaLoad, 'Awake', nil)



