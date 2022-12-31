import pyscipopt as opt

model = opt.Model()
import pymysql
import sys
import math
import numpy as np

# 数据库连接
# try:
conn = pymysql.connect(host='localhost', user='root', port=3306, password='535667',
                       database='oilblend', charset='utf8')
# 使用cursor()方法获取操作游标
cursor = conn.cursor()
# txt文件存储位置
self_FilePath = r"E:\Python_env"

# 数据库获取

# SQL 查询语句
sql_comp = "SELECT * FROM compoilconfig"
sql_weight = "SELECT weight FROM dispatchweight"
sql_prod = "SELECT * FROM prodoilconfig"
# try:
# 执行SQL语句
cursor.execute(sql_weight)
# 获取所有记录列表
results_weight_tuple = cursor.fetchall()
results_weight_array = np.array(results_weight_tuple)

cursor.execute(sql_comp)
results_comp_tuple = cursor.fetchall()

cursor.execute(sql_prod)
results_prod_tuple = cursor.fetchall()

N = len(results_comp_tuple)  # 组分油个数
P = int(results_weight_array[5])  # 成品油个数
J = 4  # 属性个数
T = int(results_weight_array[0])  # 优化周期

# 车柴产量最大权值
# 十六烷值卡边权值
# 出柴产量最大权值
# 调度方案切换权值
weight1 = results_weight_array[1]
weight2 = results_weight_array[2]
weight3 = results_weight_array[3]
weight4 = results_weight_array[4]

pro = []
# 组分油属性
for i in range(N):
    pro.append([])
    pro[i].append(results_comp_tuple[i][2])  # CET
    pro[i].append(results_comp_tuple[i][3])  # D50
    pro[i].append(results_comp_tuple[i][4])  # POL
    pro[i].append(results_comp_tuple[i][5])  # DEN

cplan_t = []
# 组分油计划产量
for i in range(T):
    cplan_t.append([])
    for j in range(N):
        #  pro.append()
        cplan_t[i].append(results_comp_tuple[j][18 + i])

# 参调流量上限
flow_max = []
for i in range(P):
    flow_max.append([])
    for j in range(N):
        if i == 0:
            flow_max[i].append(results_comp_tuple[j][12])
        if i == 1:
            flow_max[i].append(results_comp_tuple[j][14])
        if i == 2:
            flow_max[i].append(results_comp_tuple[j][30])
        if i == 3:
            flow_max[i].append(results_comp_tuple[j][32])

flow_min = []
for i in range(P):
    flow_min.append([])
    for j in range(N):
        if i == 0:
            flow_min[i].append(results_comp_tuple[j][11])
        if i == 1:
            flow_min[i].append(results_comp_tuple[j][13])
        if i == 2:
            flow_min[i].append(results_comp_tuple[j][29])
        if i == 3:
            flow_min[i].append(results_comp_tuple[j][31])
# 最小流量值
x_min = 100

# 组分油属性
pro_up = []
for i in range(P):
    pro_up.append([])
    pro_up[i].append(results_prod_tuple[i][3])
    pro_up[i].append(results_prod_tuple[i][5])
    pro_up[i].append(results_prod_tuple[i][7])
    pro_up[i].append(results_prod_tuple[i][9])

pro_low = []
for i in range(P):
    pro_low.append([])
    #  pro.append()
    pro_low[i].append(results_prod_tuple[i][2])
    pro_low[i].append(results_prod_tuple[i][4])
    pro_low[i].append(results_prod_tuple[i][6])
    pro_low[i].append(results_prod_tuple[i][8])

# 组分油库存
cs_t_min = []
cs_t_max = []
cs_chu = []

for i in range(N):
    #  pro.append()
    cs_t_min.append(results_comp_tuple[i][17])
    cs_t_max.append(results_comp_tuple[i][16])
    cs_chu.append(results_comp_tuple[i][15])

# 产品库存
ps_t_min = []
ps_t_max = []
ps_chu = []

for i in range(P):
    #  pro.append()
    ps_t_min.append(results_prod_tuple[i][11])
    ps_t_max.append(results_prod_tuple[i][12])
    ps_chu.append(results_prod_tuple[i][10])

# 产品需求 带周期7
p_dmin = []
for i in range(T):
    p_dmin.append([])
    p_dmin[i].append(results_prod_tuple[0][13 + 2 * i])
    p_dmin[i].append(results_prod_tuple[1][13 + 2 * i])
    p_dmin[i].append(results_prod_tuple[2][13 + 2 * i])
    p_dmin[i].append(results_prod_tuple[3][13 + 2 * i])
p_dmax = []
for i in range(T):
    p_dmax.append([])
    p_dmax[i].append(results_prod_tuple[0][14 + 2 * i])
    p_dmax[i].append(results_prod_tuple[1][14 + 2 * i])
    p_dmax[i].append(results_prod_tuple[2][14 + 2 * i])
    p_dmax[i].append(results_prod_tuple[3][14 + 2 * i])

# 求解器添加约束
x = []
for t in range(T):
    x1 = []
    for p in range(P):
        x2 = []
        for n in range(N):
            x2.append(model.addVar(lb=flow_min[p][n], ub=flow_max[p][n], vtype="C",
                                   name="组分油流量_" + str(t) + "_" + str(p) + "_" + str(n)))
        x1.append(x2)
    x.append(x1)

y = []
for t in range(T):
    y1 = []
    for p in range(P):
        y12 = []
        for n in range(N):
            y12.append(model.addVar(vtype="B", name="组分油0-1变量_" + str(t) + "_" + str(p) + "_" + str(n)))
        y1.append(y12)
    y.append(y1)

# 配方是否变化的0-1变量yb
yb = []
for t in range(T - 1):
    yb1 = []
    for p in range(P):
        yb2 = []
        for n in range(N):
            yb2.append(
                model.addVar(vtype="B", name="组分油流量变化0-1变量_" + str(t) + "_" + str(p) + "_" + str(n)))
        yb1.append(yb2)
    yb.append(yb1)

# t阶段末的组分油库存变量
cs_tc = []
for t in range(T):
    cs_tc1 = []
    for n in range(N):
        cs_tc1.append(
            model.addVar(lb=cs_t_min[n], ub=cs_t_max[n], vtype="C", name="组分油库存_" + str(t) + "_" + str(n)))
    cs_tc.append(cs_tc1)

# t阶段末的产品库存变量
ps_tp = []
for t in range(T):
    ps_tp1 = []
    for p in range(P):
        ps_tp1.append(
            model.addVar(lb=ps_t_min[p], ub=ps_t_max[p], vtype="C", name="产品库存_" + str(t) + "_" + str(p)))
    ps_tp.append(ps_tp1)

# 提货量
lp = []
for t in range(T):
    lp1 = []
    for p in range(P):
        lp1.append(
            model.addVar(lb=p_dmin[t][p], ub=p_dmax[t][p], vtype="C", name="提货量_" + str(t) + "_" + str(p)))
    lp.append(lp1)

M = 10e5
# 约束组分油参调0-1变量
for t in range(T):
    for p in range(P):
        for n in range(N):
            model.addCons(x[t][p][n] * M >= y[t][p][n])
            model.addCons(x[t][p][n] <= y[t][p][n] * M)
            model.addCons((x[t][p][n] - x_min) * y[t][p][n] >= 0)

# 约束2-3 组分油库存约束
for t in [0]:
    for n in range(N):
        lhs77 = 0
        lhs99 = 0
        for p in range(P):
            lhs77 = lhs77 + x[t][p][n]
        lhs99 = lhs77 + cs_tc[t][n] - cs_chu[n] - cplan_t[t][n]
        model.addCons(lhs99 == 0)

for t in range(1, T):
    for n in range(N):
        lhs7 = 0
        lhs9 = 0
        for p in range(P):
            lhs7 = lhs7 + x[t][p][n]
        lhs9 = lhs7 + cs_tc[t][n] - cs_tc[t - 1][n] - cplan_t[t][n]
        model.addCons(lhs9 == 0)

# 约束5-6 产品库存约束
for t in [0]:
    for p in range(P):
        lhs100 = 0
        lhs120 = 0
        for n in range(N):
            lhs100 = lhs100 + x[t][p][n]
        lhs120 = ps_chu[p] + lhs100 - lp[t][p] - ps_tp[t][p]
        model.addCons(lhs120 == 0)

#
for t in range(1, T):
    for p in range(P):
        lhs10 = 0
        lhs12 = 0
        for n in range(N):
            lhs10 = lhs10 + x[t][p][n]
        lhs12 = ps_tp[t - 1][p] + lhs10 - lp[t][p] - ps_tp[t][p]
        model.addCons(lhs12 == 0)

# 约束8 属性约束 十六烷值、多环芳烃含量、密度、馏程
# 取对数math.log(x, 10) 以10为底x的对数
# 幂次方 math.pow(100, 2) 100的²
for t in range(T):
    for p in range(P):
        for j in range(J):  # 属性个数
            lhs3 = 0
            # lhs3_1 = 0
            sum_f1 = 0
            for n in range(N):
                lhs3 = lhs3 + x[t][p][n] * pro[n][j]
                # lhs3_1 = lhs3 + x[t][p][n] * pro[n][j]
                sum_f1 = sum_f1 + x[t][p][n]
            # 产品低限
            model.addCons(lhs3 >= pro_low[p][j] * sum_f1)
            # 产品高限
            model.addCons(lhs3 <= pro_up[p][j] * sum_f1)

# 调度方案切换约束，体现在相应变量是否变化
for t in range(T - 1):
    for p in range(P):
        for n in range(N):
            model.addCons((x[t][p][n] - x[t + 1][p][n]) * (1 - yb[t][p][n]) == 0)

# 计算十六烷值差值
pro_sl = []
for t in range(T):
    lhs4 = 0
    sum_f2 = 0
    for n in range(N):
        lhs4 = lhs4 + x[t][1][n] * pro[n][0]
        sum_f2 = sum_f2 + x[t][1][n]
    lhs4 = lhs4 - pro_low[1][0] * sum_f2
    pro_sl.append(lhs4)

# 计算车柴产量
flow_che = []
for t in range(T):
    for p in range(P):
        lhs5 = 0
        for n in range(N):
            lhs5 = lhs5 + x[t][0][n]
        flow_che.append(lhs5)
# 计算出柴产量
flow_chu = []
for t in range(T):
    for p in range(P):
        lhs6 = 0
        for n in range(N):
            lhs6 = lhs6 + x[t][1][n]
        flow_chu.append(lhs6)
# 计算调度方案数值
diaodu = []
for t in range(T - 1):
    for p in range(P):
        lhs44 = 0
        for n in range(N):
            lhs44 = lhs44 + yb[t][p][n]
        diaodu.append(lhs44)

# 设置目标函数
# 目标函数
model.setObjective(
    sum(flow_che[t] for t in range(T)) * weight1 - sum(pro_sl[t] for t in range(T)) * weight2 +
    sum(flow_chu[t] for t in range(T)) * weight3 - sum(diaodu[t] for t in range(T - 1)) * weight4, "maximize")

# model.hideOutput(True)  # 隐藏求解日志
model.optimize()
scip_status = model.getStatus()  # 求解标志位
# print(model.getStatus)
# print("目标函数值:", model.getObjVal())

# 数据库写入
if scip_status == 'optimal':  # 求解成功
    # # 获取优化结果
    # 组分油车柴参调和出柴参调
    # flow = []
    # # print("流量值:")
    # for t in range(T):
    #     flow.append([])
    #     for p in range(P):  # P是产品个数，即出柴和车柴
    #         for n in range(N):  # N是组分油个数
    #             flow[t].append(model.getVal(x[t][p][n]))
    flow = []
    for t in range(T):
        for p in range(P):
            for n in range(N):
                flow.append(round(model.getVal(x[t][p][n])))
    flow_txt = np.array([flow])
    np.savetxt(self_FilePath + r"\Flow_txt.txt", flow_txt, fmt='%d', delimiter=',')
    print(flow)

    # 组分油库存
    # ComOil_Inv = []
    # # print("组分油库存变量cs_tc:")
    # for t in range(T):
    #     ComOil_Inv.append([])
    #     for n in range(N):
    #         ComOil_Inv[t].append(model.getVal(cs_tc[t][n]))

    ComOil_Inv = []
    # print("组分油库存变量cs_tc:")
    for t in range(T):
        for n in range(N):
            ComOil_Inv.append(round(model.getVal(cs_tc[t][n])))
    ComOil_Inv_txt = np.array([ComOil_Inv])
    np.savetxt(self_FilePath + r"\ComOil_Inv_txt.txt", ComOil_Inv_txt, fmt='%d', delimiter=',')
    print(ComOil_Inv)

    # 成品油库存
    # ProdOil_Inv = []
    # for t in range(T):
    #     ProdOil_Inv.append([])
    #     for p in range(P):
    #         ProdOil_Inv[t].append(model.getVal(ps_tp[t][p]))

    ProdOil_Inv = []
    for t in range(T):
        for p in range(P):
            ProdOil_Inv.append(round(model.getVal(ps_tp[t][p])))
    ProdOil_Inv_txt = np.array([ProdOil_Inv])
    np.savetxt(self_FilePath + r"\ProdOil_Inv_txt.txt", ProdOil_Inv_txt, fmt='%d', delimiter=',')
    print(ProdOil_Inv)

    # 提货量
    # Prod_LP = []
    # for t in range(T):
    #     Prod_LP.append([])
    #     for p in range(P):
    #         Prod_LP[t].append(model.getVal(lp[t][p]))

    Prod_LP = []
    for t in range(T):
        for p in range(P):
            Prod_LP.append(round(model.getVal(lp[t][p])))
    Prod_LP_txt = np.array([Prod_LP])
    np.savetxt(self_FilePath + r"\Prod_LP_txt.txt", Prod_LP_txt, fmt='%d', delimiter=',')
    print(Prod_LP)

    Status_txt = []
    Status_txt.append('计算成功')
    np.savetxt(self_FilePath + r"\Status_txt.txt", Status_txt, encoding='utf-8', fmt='%s', delimiter=',')
    print(Status_txt)

    Obj_txt = []
    Obj_txt.append(model.getObjVal())
    np.savetxt(self_FilePath + r"\Obj_txt.txt", Obj_txt, fmt='%d', delimiter=',')
    print(Obj_txt)

    sys.exit()

elif scip_status == 'infeasible':
    flow = []
    for t in range(T):
        for p in range(P):
            for n in range(N):
                flow.append(0)
    flow_txt = np.array([flow])
    np.savetxt(self_FilePath + r"\Flow_txt.txt", flow_txt, fmt='%d', delimiter=',')
    print(flow)

    # 组分油库存
    ComOil_Inv = []
    # print("组分油库存变量cs_tc:")
    for t in range(T):
        for n in range(N):
            ComOil_Inv.append(0)
    ComOil_Inv_txt = np.array([ComOil_Inv])
    np.savetxt(self_FilePath + r"\ComOil_Inv_txt.txt", ComOil_Inv_txt, fmt='%d', delimiter=',')
    print(ComOil_Inv)

    # 成品油库存
    ProdOil_Inv = []
    for t in range(T):
        for p in range(P):
            ProdOil_Inv.append(0)
    ProdOil_Inv_txt = np.array([ProdOil_Inv])
    np.savetxt(self_FilePath + r"\ProdOil_Inv_txt.txt", ProdOil_Inv_txt, fmt='%d', delimiter=',')
    print(ProdOil_Inv)

    # 提货量
    Prod_LP = []
    for t in range(T):
        for p in range(P):
            Prod_LP.append(0)
    Prod_LP_txt = np.array([Prod_LP])
    np.savetxt(self_FilePath + r"\Prod_LP_txt.txt", Prod_LP_txt, fmt='%d', delimiter=',')
    print(Prod_LP)

    Status_txt = []
    Status_txt.append('计算失败')
    np.savetxt(self_FilePath + r"\Status_txt.txt", Status_txt, encoding='utf-8', fmt='%s', delimiter=',')
    print(Status_txt)

    Obj_txt = []
    Obj_txt.append(0)
    np.savetxt(self_FilePath + r"\Obj_txt.txt", Obj_txt, fmt='%d', delimiter=',')
    print(Obj_txt)

    sys.exit()

else:
    flow = []
    for t in range(T):
        for p in range(P):
            for n in range(N):
                flow.append(0)
    flow_txt = np.array([flow])
    np.savetxt(self_FilePath + r"\Flow_txt.txt", flow_txt, fmt='%d', delimiter=',')
    print(flow)

    # 组分油库存
    ComOil_Inv = []
    # print("组分油库存变量cs_tc:")
    for t in range(T):
        for n in range(N):
            ComOil_Inv.append(0)
    ComOil_Inv_txt = np.array([ComOil_Inv])
    np.savetxt(self_FilePath + r"\ComOil_Inv_txt.txt", ComOil_Inv_txt, fmt='%d', delimiter=',')
    print(ComOil_Inv)

    # 成品油库存
    ProdOil_Inv = []
    for t in range(T):
        for p in range(P):
            ProdOil_Inv.append(0)
    ProdOil_Inv_txt = np.array([ProdOil_Inv])
    np.savetxt(self_FilePath + r"\ProdOil_Inv_txt.txt", ProdOil_Inv_txt, fmt='%d', delimiter=',')
    print(ProdOil_Inv)

    # 提货量
    Prod_LP = []
    for t in range(T):
        for p in range(P):
            Prod_LP.append(0)
    Prod_LP_txt = np.array([Prod_LP])
    np.savetxt(self_FilePath + r"\Prod_LP_txt.txt", Prod_LP_txt, fmt='%d', delimiter=',')
    print(Prod_LP)

    Status_txt = []
    Status_txt.append('计算失败：' + scip_status)
    np.savetxt(self_FilePath + r"\Status_txt.txt", Status_txt, encoding='utf-8', fmt='%s', delimiter=',')
    print(Status_txt)

    Obj_txt = []
    Obj_txt.append(0)
    np.savetxt(self_FilePath + r"\Obj_txt.txt", Obj_txt, fmt='%d', delimiter=',')
    print(Obj_txt)

    sys.exit()

# except Exception as e:
#     print(e)  # 数据库写入错误
#     sys.exit()
