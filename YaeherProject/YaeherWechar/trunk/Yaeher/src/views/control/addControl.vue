<template>
    <div class="add-control padding-top">
        <mt-header fixed title="添加质控委员">
            <a @click="$router.go(-1)" slot="left">
                <mt-button icon="back">返回</mt-button>
            </a>
        </mt-header>
        <div class="content listAbout">
            <div class="searchCase">
                <div class="searchBox">
                    <input @keypress="searchKeypress" ref="inputText" type="search" v-model="keyWord" placeholder="请输入医生名查找" />
                    <div @click="getYaeherClinicDoctors(true)">搜索</div>
                </div>
            </div>
            <ul v-infinite-scroll="getYaeherClinicDoctors"
  				infinite-scroll-disabled="loading"
  				infinite-scroll-distance="10" class="group2-list">
                <li v-for="(item, index) in doctorList" :key="index">
                    <div class="flex doctorInfo">
                        <router-link :to="{path: '/doctor-detail-patient', query: {id: item.id}}" class="img-case">
                            <img :src="item.userImage" align="avater" />
                        </router-link>
                        <div class="flexCase">
                            <p class="doctor-name">
                                {{item.doctorName}}
                            </p>
                            <p class="star-font"><star :star=item.AverageEvaluate></star> {{item.AverageEvaluate}}</p>
                            <p><span>{{item.hospitalName}}</span> <span>{{item.title}}</span></p>
                            <div v-if="item.qualityControlId === 0" class="operation-control">
                                <mt-button @click="hintBtn(item.id,'您确定设置为质控委员？')" type="primary">设置为质控委员</mt-button>
                            </div>
                            <div v-if="item.qualityControlId != 0" class="operation-control">
                                <mt-button type="danger">已经是质控委员</mt-button>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import { MessageBox, Toast } from 'mint-ui';
import Star from 'components/star/star'
export default {
    components: {
      Star
    },
    data () {
        return {
            keyWord: '',
            doctorList: [],
            skipCount: 1,
			maxResultCount: 10,
            totalPage: 2,
            loading: true,
        }
    },
    methods: {
        hintBtn(id, mesg) {
            const doctorID = id
            MessageBox.confirm(mesg).then(action => { // 设置质控委员
                this.instance.createQualityCommitteeC({
                    doctorID
                })
                    .then((response) => {
                        console.log(response.data.result.code === 200)
                        if(response.data.result.code === 200) {
                            Toast('设置成功')
                            this.getYaeherClinicDoctors()
                        } else {
                            cosole.log('else')
                        }
                    })
                    .catch((error) => {
                        console.log('设置失败')
                    }) 
            },function(){
                console.log('取消了')
            })
        },
        searchKeypress() {
            if (event.keyCode == 13) {
                this.getYaeherClinicDoctors(true)
            } 
        },
        getYaeherClinicDoctors(flag) { // 获取医生列表
            this.loading = true
            if (flag) {
                this.skipCount = 1
                this.totalPage = 2
            }
            const keyWord = this.keyWord
            const maxResultCount  = this.maxResultCount 
            const skipCount = this.skipCount
            
			if (skipCount > this.totalPage) {
				if(skipCount > 2) {
                    Toast('没有更多了！')
                }
				return
			}
            this.$refs.inputText.blur()
            this.instance.doctorSearch({
                keyWord,
                skipCount,
                maxResultCount,
                onlineState: ''
            })
                .then((response) => {
                    setTimeout(() => {
                        this.loading = false
                        let moreFlag = response.data.result.item.items
                        if (!moreFlag || moreFlag.length===0) {
							this.loading = true
                        }
                        if (flag) this.doctorList = []
                        this.doctorList = this.doctorList.concat(response.data.result.item.items)
                        this.skipCount ++
                        this.totalPage = response.data.result.item.totalPage
                        console.log(this.skipCount, this.totalPage)
                    }, 100);    
                })
                .catch((error) => {
                    this.loading = true
                    console.log('医生列表请求失败')
                }) 
        },
        
    },
    mounted () {
        this.getYaeherClinicDoctors()
    }
}
</script>

<style lang="scss">

@import "~assets/sass/base.scss";
.add-control {
    .listAbout {
        position: absolute; left: 0; right: 0; bottom: 0px;top: 42px;
    }
    .group2-list { 
        font-size: $font-m;
        color: $color-bfont;
        background: $color-wfont;
        li {
            padding: 15px 0; 
            display: flex;
            .doctorInfo {
                width: 100%;
                .img-case {
                    width: 60px;
                    height: 60px;
                    display: block;
                    img{
                        display: block;
                        width: 60px;
                        height: 60px;
                        border-radius: 5px;
                    }
                }
                .flexCase {
                    padding: 0 10px; 
                    position: relative;
                    flex: 1;
                    p {
                        font-size: $font-m;
                        color: $color-bfont;
                        padding-bottom: 2px;
                    }
                    .doctor-name {
                        font-size: $font-xl;
                        color: $color-afont;
                    }
                    .star-font {
                        color: $color-star;
                    }
                    .operation-control {
                        position: absolute;
                        right: 0;
                        top: 10px;
                        .mint-button {
                            height: 30px;
                            font-size: $font-l;
                            margin-left: 10px;
                        }
                    }
                }
            } 
        }
    }
}


</style>