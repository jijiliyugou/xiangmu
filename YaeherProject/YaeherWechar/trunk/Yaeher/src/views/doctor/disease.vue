<template>
    <div class="disease padding-top">
        <mt-header fixed title="病种">
            <a @click="$router.push({path: '/doctor-info-list'})" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
            <mt-button  slot="right">
                <router-link to="/add-disease" class="right-white">添加</router-link>
            </mt-button> 
        </mt-header>
        <div class="content disease-box">
            <ul class="disease-list">
                <li class="diseaseHint red-hint">
                    温馨提示：病种选择顺序即为对外擅长治疗疾病标签的展示顺序
                </li>
                <li v-for="(item, index) in lableList" :key="index">
                    <div class="flex">
                        <div class="disease-name">{{item.lableName}}</div>
                        <div class="disease-operate">
                            <mt-button @click="deleteLable(item.lableID, item.lableName)" type="default">删除</mt-button>
                        </div>
                    </div>
                </li>
            </ul>
            
        </div>
    </div>
</template>

<script>
import { Toast, MessageBox } from 'mint-ui';
export default {
    data () {
        return {
            id: 0,
            lableList: [],
            maxResultCount: 100
        }
    },
    methods: {
        getlablePage() { // 获取病种列表
            const maxResultCount = this.maxResultCount
            this.instance.lablePageD({
                maxResultCount
            })
                .then((response) => {
                    this.lableList = response.data.result.item.items
                })
                .catch((error) => {
                }) 
        },
        deleteLable(id1, lableName) { // 删除病种
            let id = id1
            MessageBox.confirm(`您确定删除 (${lableName}) 病种？`).then(action => {
                this.instance.deleteLableD({
                    id
                })
                    .then((response) => {
                        Toast('删除成功')
                        this.getlablePage()
                    })
                    .catch((error) => {
                    }) 
                },function(){
                    console.log('取消了');
                })
            
        }
    },
    mounted () {
        this.getlablePage()
    }
}
</script>

<style lang="scss">
@import "~assets/sass/base.scss";

.disease {
    .disease-box {
        .disease-list {
            margin: 10px;
            padding: 0;
            li {
                padding: 7px 10px;
                .disease-name {
                    line-height: 30px;
                }
                .disease-operate {
                    text-align: right;
                    padding-right: 10px;
                    .mint-button {
                        height: 30px;
                        font-size: $font-l;
                    }
                }
                
            }
            .diseaseHint {
                font-size: $font-m;
                line-height: 20px;
            }
        }
    }
}



</style>