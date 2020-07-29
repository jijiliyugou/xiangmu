<template>
    <div class="control-list padding-top">
        <mt-header fixed title="质控委员列表">
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
                            <p class="star-font"><star :star=item.averageEvaluate></star> {{item.averageEvaluate}}</p>
                            <p><span>{{item.hospitalName}}</span> <span>{{item.title}}</span></p>
                            <div class="operation-control">
                                <mt-button v-if="id!=0" @click="hintBtn(item.id, true, '您确定转给质控委员？')" type="default">转给</mt-button>
                                <mt-button @click="hintBtn(item.qualityControlId, false, '您确定删除质控委员？')" type="default">删除</mt-button>
                            </div>
                        </div>
                    </div>
                </li>
            </ul>        
        </div>
    </div>
</template>

<script>
import Star from 'components/star/star'
import { Toast, MessageBox } from 'mint-ui'
import { createSecret } from 'assets/js/common.js'
import searchCase from 'components/search/searchCase'
export default {
    components: {
      Star,
      searchCase
    },
    data () {
        return {
            searchValue: '请输入医生名或者标签',
            doctorList: [],
            id: 0,
            skipCount: 1,
			maxResultCount: 10,
			totalPage: 2,
            keyWord: '',
            loading: true
        }
    },
    methods: {
        hintBtn(id1, flag, mesg) {
            if (flag) {
                MessageBox.confirm(mesg).then(action => {
                    const consultID = this.id
                    const doctorID = id1
                    this.instance.createQualityControlManageC({ // 转给质控委员
                        consultID,
                        doctorID
                    })
                        .then((response) => {
                            if(response.data.result.code === 200) {
                                Toast('转给成功')
                                this.$router.go(-1)
                            }
                            
                        })
                        .catch((error) => {
                            console.log('转给失败')
                        }) 
                },function(){
                    console.log('取消了');
                })
            } else {
                MessageBox.confirm(mesg).then(action => {
                    const id = id1
                    this.instance.celeteQualityCommitteeC({ // 删除质控委员
                        id
                    })
                        .then((response) => {
                            if(response.data.result.code === 200) {
                                Toast('删除成功')
                                this.getYaeherClinicDoctors()
                            }
                            
                        })
                        .catch((error) => {
                            console.log('删除失败')
                        }) 
                    
                },function(){
                    console.log('取消了');
                })
            }
        },
        getYaeherClinicDoctors(flag) { // 获取医生列表
            this.loading = true
            this.$refs.inputText.blur()
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
            this.instance.qualityCommitteePageC({
                keyWord,
                skipCount,
                maxResultCount
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
                    console.log('获取质控委员列表失败')
                }) 
        },
        searchKeypress (event) {
            if (event.keyCode == 13) {
                this.getYaeherClinicDoctors(true)
            } 
        }
        
    },
    mounted () {
        this.getYaeherClinicDoctors()
    },
    created () {
        let id = this.$route.query.id
        if (id) {
            this.id = parseInt(id)
        }
    }
}
</script>

<style lang="scss">

@import "~assets/sass/base.scss";
.control-list {
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